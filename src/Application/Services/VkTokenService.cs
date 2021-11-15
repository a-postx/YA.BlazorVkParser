using Microsoft.AspNetCore.Components;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YA.Common.Extensions;
using YA.WebClient.Application.Models.Queries;
using YA.WebClient.Extensions;
using YA.WebClient.Options;

namespace YA.WebClient.Application.Services;

public class VkTokenService : IVkTokenService
{
    public VkTokenService(IJSRuntime jsRuntime,
        IApiRepository apiRepository,
        NavigationManager navigationManager,
        VkontakteOptions vkOptions)
    {
        _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        _vkOptions = vkOptions ?? throw new ArgumentNullException(nameof(vkOptions));
    }

    private readonly IJSRuntime _js;
    private readonly IApiRepository _apiRepository;
    private readonly NavigationManager _navigationManager;
    private readonly VkontakteOptions _vkOptions;

    public async Task GetVkCode(string redirectAddress)
    {
        await _js.ConsoleLog("идём в ВК за кодом");

        string rightsScope = "offline+ads+photos+video+wall+market+groups";
        string redirectUrl = _navigationManager.BaseUri + _vkOptions.VkCodeRequestRedirectUrlPath;
        string responseType = "code";

        string clientRequestId = Guid.NewGuid().ToString();

        VkCodeRequestState state = new VkCodeRequestState { ClientRequestId = clientRequestId, RedirectAddress = redirectAddress };

        string serializedState = JsonSerializer
            .Serialize(state, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
        string requestState = serializedState.Base64Encode();

        string fullUrl = $"{_vkOptions.VkAuthorizationRequestUrl}?scope={rightsScope}&redirect_uri={redirectUrl}&client_id={_vkOptions.VkApplicationId}&response_type={responseType}&state={requestState}";

        _navigationManager.NavigateTo(fullUrl);
    }

    public async Task<(VkAccessTokenVm, string, Guid)> RequestAndSaveVkAccessToken(string code)
    {
        string redirectAddress = _navigationManager.BaseUri + _vkOptions.VkCodeRequestRedirectUrlPath;

        ApiCommandResult<VkAccessTokenVm> result = await _apiRepository
            .RequestAndCreateVkAccessToken(_vkOptions.VkApplicationId, redirectAddress, code);

        switch (result.Status)
        {
            case ApiCommandStatus.Ok:
                return (result.Data, string.Empty, Guid.Empty);
            case ApiCommandStatus.UnprocessableEntity:
                return (null, result.ErrorText, result.RequestId);
            default:
                await _js.ConsoleLog("Ошибка получения и сохранения токена ВК. " + result.ErrorText);
                return (null, "Ошибка получения и сохранения токена ВК. " + result.ErrorText, result.RequestId);
        }
    }
}
