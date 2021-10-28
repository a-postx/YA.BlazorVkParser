using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using YA.WebClient.Application;
using YA.WebClient.Application.Enums;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Application.Models.Internal;
using YA.WebClient.Constants;
using YA.WebClient.Extensions;

namespace YA.WebClient.Infrastructure.HttpContext
{
    public sealed class ApiHttpContext : IApiHttpContext
    {
        public ApiHttpContext(NavigationManager navigation,
            IJSRuntime jsRuntime,
            IHttpClientFactory httpClientFactory,
            ITokenService tokenService,
            ISignService signService)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _signService = signService ?? throw new ArgumentNullException(nameof(signService));
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly NavigationManager _navigation;
        private readonly IJSRuntime _js;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
        private readonly ISignService _signService;

        public event EventHandler<EventArgs> TenantSelectionRequired;

        private void PublishTenantSelectionRequiredEvent()
        {
            TenantSelectionRequired?.Invoke(this, null);
        }

        public Task<ApiCommandResult<T>> PostAsync<T>(Uri url, object data, int timeout, bool auth = true, bool useBackgroundClient = false)
        {
            string clientName = useBackgroundClient ? "background" : "foreground";

            HttpClient client = _httpClientFactory.CreateClient(clientName);

            //добавить логику повторения с тем же ключом в случае проблем
            SetIdempotencyHeader(client);

            return SendAsync<T>(() => client.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, General.DefaultMediaType),
                new CancellationTokenSource(timeout).Token)
                , client, auth);
        }

        public Task<ApiCommandResult<T>> GetAsync<T>(Uri url, int timeout, bool auth = true, bool useBackgroundClient = false)
        {
            string clientName = useBackgroundClient ? "background" : "foreground";

            HttpClient client = _httpClientFactory.CreateClient(clientName);
            return SendAsync<T>(() => client.GetAsync(url, new CancellationTokenSource(timeout).Token), client, auth);
        }

        public Task<ApiCommandResult<T>> DeleteAsync<T>(Uri url, int timeout, bool auth = true)
        {
            HttpClient foregroundClient = _httpClientFactory.CreateClient("foreground");
            return SendAsync<T>(() => foregroundClient.DeleteAsync(url, new CancellationTokenSource(timeout).Token), foregroundClient, auth);
        }

        public Task<ApiCommandResult<T>> PatchAsync<T>(Uri url, object data, int timeout, bool auth = true, bool useBackgroundClient = false)
        {
            string clientName = useBackgroundClient ? "background" : "foreground";

            HttpClient foregroundClient = _httpClientFactory.CreateClient(clientName);

            //добавить логику повторения с тем же ключом в случае проблем
            SetIdempotencyHeader(foregroundClient);

            return SendAsync<T>(() => foregroundClient.PatchAsync(url,
                //Сериализатор System.Text.Json не поддерживает патчи и не планирует
                //создаёт объект
                //'{"Operations":[{"value":false,"OperationType":2,"path":"/Autodelete","op":"replace","from":null}],"ContractResolver":{}}'
                //вместо
                //'[{"value":false,"path":"/Autodelete","op":"replace"}]'
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, General.DefaultMediaType),
                new CancellationTokenSource(timeout).Token),
                foregroundClient, auth);
        }

        public Task<ApiCommandResult<T>> PutAsync<T>(Uri url, object data, int timeout, bool auth = true)
        {
            HttpClient foregroundClient = _httpClientFactory.CreateClient("foreground");
            return SendAsync<T>(() => foregroundClient.PutAsync(url,
                new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, General.DefaultMediaType), new CancellationTokenSource(timeout).Token),
                foregroundClient,
                auth);
        }

        private async Task<ApiCommandResult<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> request, HttpClient client, bool needAuth)
        {
            if (needAuth)
            {
                string jwt = await _tokenService.GetTokenAsync();

                if (!string.IsNullOrEmpty(jwt))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                }
                else
                {
                    await _js.ConsoleLog($"токена нет, УРЛ {_navigation.Uri}");

                    return new ApiCommandResult<T>(ApiCommandStatus.Unauthorized, default(T), "Вы не авторизованы!",
                        Guid.Parse("00000000-0000-0000-0000-000000000001"));
                }
            }

            HttpResponseMessage response = null;
            string requestException = string.Empty;

            try
            {
                response = await request();
            }
            catch (Exception ex)
            {
                await _js.ConsoleLog(ex.Message);
                requestException = "Произошла сетевая ошибка: " + ex.Message;
            }

            if (response != null)
            {
                (T result, int statusCode, string error, Guid requestId) = await Handle<T>(response);

                switch (statusCode)
                {
                    //https://ocelot.readthedocs.io/en/latest/features/errorcodes.html
                    default:
                        return new ApiCommandResult<T>(ApiCommandStatus.Unknown, result, error, requestId);
                    case 200:
                    case 201:
                    case 204:
                        return new ApiCommandResult<T>(ApiCommandStatus.Ok, result, error, requestId);
                    case 404:
                        return new ApiCommandResult<T>(ApiCommandStatus.NotFound, result, error, requestId);
                    case 412:
                        return new ApiCommandResult<T>(ApiCommandStatus.ConcurrencyIssue, result, error, requestId);
                    case 422:
                        return new ApiCommandResult<T>(ApiCommandStatus.UnprocessableEntity, result, error, requestId);
                    case 500:
                        return new ApiCommandResult<T>(ApiCommandStatus.InternalServerError, result, error, requestId);
                    case 502:
                        return new ApiCommandResult<T>(ApiCommandStatus.BadGateway, result, error, requestId);
                    case 503:
                        return new ApiCommandResult<T>(ApiCommandStatus.ServiceUnavailable, result, error, requestId);
                }
            }
            else
            {
                //как отличить недоступность интернета от недоступности шлюза?
                //если ошибка сети (fetch), то показывать внизу панельку Офлайн?
                return new ApiCommandResult<T>(ApiCommandStatus.UnableToMakeApiCall, default(T), requestException, Guid.Empty);
            }
        }

        private async Task<(T, int, string, Guid)> Handle<T>(HttpResponseMessage response)
        {
            Guid requestId = Guid.Empty;

            if (response.Headers.TryGetValues("x-ya-request-id".ToUpperInvariant(), out IEnumerable<string> values))
            {
                string rawRequestId = values.FirstOrDefault();

                if (!string.IsNullOrEmpty(rawRequestId) && Guid.TryParse(rawRequestId, out Guid parsedReqId))
                {
                    requestId = parsedReqId;
                }
            }

            string json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Guid tenantIdFromToken = await _tokenService.GetTenantIdAsync();

                    if (tenantIdFromToken != Guid.Empty)
                    {
                        await _js.ConsoleLog($"перенаправляем пользователя на выход, адрес возвращения {_navigation.Uri}");

                        await _signService.BeginLogoutAsync();
                    }
                    else
                    {
                        PublishTenantSelectionRequiredEvent();
                    }
                }
                
                if (string.IsNullOrWhiteSpace(json))
                {
                    return (default(T), (int)response.StatusCode, response.StatusCode.ToString(), requestId);
                }
                else
                {
                    try
                    {
                        YaProblemDetails pdObject = null;

                        pdObject = JsonSerializer.Deserialize<YaProblemDetails>(json, Options);
                        string problemTitle = pdObject != null ? pdObject.Title : string.Empty;

                        //дописать отображение деталей проблемы валидации, которые пользователь может устранить
                        return (default(T), (int)response.StatusCode, problemTitle, requestId);
                    }
                    catch (Exception)
                    {
                        return (default(T), (int)response.StatusCode, null, requestId);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(json))
                {
                    T resultObject = JsonSerializer.Deserialize<T>(json, Options);
                    return (resultObject, (int)response.StatusCode, null, requestId);
                }
                else
                {
                    string errorMessage = response.StatusCode == HttpStatusCode.NoContent ? null : "В ответе отсутствует содержимое";
                    return (default(T), (int)response.StatusCode, errorMessage, requestId);
                }
            }
        }

        private static Guid SetIdempotencyHeader(HttpClient httpClient, Guid id = default)
        {
            Guid idempotencyKey = id == Guid.Empty ? Guid.NewGuid() : id;

            httpClient.DefaultRequestHeaders.Remove(General.IdempotencyHeader);
            httpClient.DefaultRequestHeaders.Add(General.IdempotencyHeader, idempotencyKey.ToString());
            return idempotencyKey;
        }
    }
}
