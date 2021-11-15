using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using System.Runtime.InteropServices;
using YA.WebClient.Application.Models.Diagnostic;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Constants;

namespace YA.WebClient.Infrastructure.Services;

public class EnvironmentContext : IEnvironmentContext
{
    public EnvironmentContext(IRuntimeGeoDataService geoDataService,
        IJSRuntime jsRuntime,
        IWebAssemblyHostEnvironment environment,
        AuthenticationStateProvider authStateProvider)
    {
        _geoDataService = geoDataService ?? throw new ArgumentNullException(nameof(geoDataService));
        _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _authStateProvider = authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
    }

    private readonly IJSRuntime _js;
    private readonly IRuntimeGeoDataService _geoDataService;
    private readonly IWebAssemblyHostEnvironment _environment;
    private readonly AuthenticationStateProvider _authStateProvider;

    public async Task<ClientInfo> GetClientInfoAsync()
    {
        using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
        {
            Task<NavigatorInfo> navigatorInfoTask = GetNavigatorInfoAsync();
            Task<UaInfo> uaInfoTask = GetUaInfoAsync();
            Task<ScreenInfo> screenInfoTask = GetScreenInfoAsync();
            Task<GeoInfo> geoInfoTask = GetGeoInfoAsync(cts.Token);

            await Task.WhenAll(navigatorInfoTask, uaInfoTask, screenInfoTask, geoInfoTask);

            ClientInfo clientInfo = new ClientInfo
            {
                Navigator = await navigatorInfoTask,
                Ua = await uaInfoTask,
                Screen = await screenInfoTask,
                Geo = await geoInfoTask
            };

            return clientInfo;
        }
    }

    private async Task<NavigatorInfo> GetNavigatorInfoAsync()
    {
        NavigatorInfo navigatorInfo = await _js.InvokeAsync<NavigatorInfo>("environmentInfo.GetNavigatorInfo");
        return navigatorInfo;
    }

    private async Task<ScreenInfo> GetScreenInfoAsync()
    {
        ScreenInfo screenInfo = await _js.InvokeAsync<ScreenInfo>("environmentInfo.GetScreenInfo");
        return screenInfo;
    }

    private async Task<UaInfo> GetUaInfoAsync()
    {
        UaInfo uaInfo = await _js.InvokeAsync<UaInfo>("environmentInfo.GetUaInfo");
        return uaInfo;
    }

    private async Task<GeoInfo> GetGeoInfoAsync(CancellationToken cancellationToken)
    {
        GeoInfo geoInfo = await _geoDataService.GetGeoInfoAsync(cancellationToken);
        return geoInfo;
    }

    public async Task<DiagnosticInfo> GetDiagnosticInfoAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();

        SecurityInfo secInfo = new SecurityInfo();

        if ((bool)authState.User?.Identity?.IsAuthenticated)
        {
            string username = authState.User.Identity.Name;

            var claims = authState.User.Claims;

            UserInfo userInfo = new UserInfo
            {
                Username = authState.User.Identity.Name,
                UserId = authState.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimNames.sub)?.Value,
                Email = authState.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimNames.email)?.Value,
                Claims = authState.User.Claims.ToDictionary(x => x.Type, y => y.Value)
            };

            secInfo.User = userInfo;
        }

        string coreClrString = ((AssemblyInformationalVersionAttribute[])typeof(object).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false))[0].InformationalVersion;
        string coreFxString = ((AssemblyInformationalVersionAttribute[])typeof(Uri).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false))[0].InformationalVersion;
        string[] clrArr = coreClrString.Split('+');
        string clrBuild = clrArr[0];
        string clrHash = clrArr[1];
        string[] fxArr = coreFxString.Split('+');
        string fxBuild = fxArr[0];
        string fxHash = fxArr[1];

        EnvironmentInfo environmentInfo = new EnvironmentInfo
        {
            HostEnvironment = _environment.Environment,
            Version = Environment.Version.ToString(),
            FrameworkVersion = RuntimeInformation.FrameworkDescription,
            RuntimeIdentifier = RuntimeInformation.RuntimeIdentifier,
            CoreClrBuild = clrBuild,
            CoreClrHash = clrHash,
            CoreFxBuild = fxBuild,
            CoreFxHash = fxHash
        };

        DiagnosticInfo diagInfo = new DiagnosticInfo
        {
            ClientName = Assembly.GetExecutingAssembly().GetName().Name,
            ClientVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
            Environment = environmentInfo,
            Security = secInfo
        };

        return diagInfo;
    }
}
