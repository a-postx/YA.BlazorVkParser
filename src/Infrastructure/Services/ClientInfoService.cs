using YA.WebClient.Application;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Extensions;

namespace YA.WebClient.Infrastructure.Services;

public class ClientInfoService : IClientInfoService
{
    public ClientInfoService(IJSRuntime jsRuntime,
        IApiRepository apiRepository,
        IEnvironmentContext environmentCtx)
    {
        _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
        _environmentCtx = environmentCtx ?? throw new ArgumentNullException(nameof(environmentCtx));
    }

    private readonly IJSRuntime _js;
    private readonly IApiRepository _apiRepository;
    private readonly IEnvironmentContext _environmentCtx;

    public async Task<(ClientInfoVm, Guid)> PublishClientInfoAsync()
    {
        ClientInfo clientInfo = await _environmentCtx.GetClientInfoAsync();

        ClientInfoSm clientInfoSm = new ClientInfoSm
        {
            ClientVersion = GetType().Assembly.GetName().Version.ToString(),
            Browser = clientInfo.Ua?.Browser?.Name,
            BrowserVersion = clientInfo.Ua?.Browser?.Version,
            Os = clientInfo.Ua?.Os?.Name,
            OsVersion = clientInfo.Ua?.Os?.Version,
            DeviceModel = clientInfo.Ua?.Device?.Model,
            ScreenResolution = $"{clientInfo.Screen?.Width}x{clientInfo.Screen?.Height}",
            ViewportSize = $"{clientInfo.Screen?.ViewportWidth}x{clientInfo.Screen?.ViewportHeight}",
            CountryName = clientInfo.Geo?.Country,
            RegionName = clientInfo.Geo?.Region,
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()
        };

        ApiCommandResult<ClientInfoVm> result = await _apiRepository.PublishClientInfo(clientInfoSm);

        switch (result.Status)
        {
            case ApiCommandStatus.Ok:
                await _js.ConsoleLog("информация о клиенте опубликована");
                return (result.Data, Guid.Empty);
            default:
                await _js.ConsoleLog("ошибка публикации информации о клиенте: " + result.ErrorText);
                return (null, result.RequestId);
        }
    }
}
