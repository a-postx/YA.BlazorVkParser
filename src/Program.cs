global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using AutoMapper;
global using System.ComponentModel;
global using Microsoft.JSInterop;
global using Microsoft.Extensions.Logging;
global using YA.WebClient.Application.Interfaces;
global using YA.WebClient.Application.Models.SaveModels;
global using YA.WebClient.Application.Models.ViewModels;
global using YA.WebClient.Application.Enums;
global using YA.WebClient.Application.Events;

using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CurrieTechnologies.Razor.PageVisibility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using YA.WebClient.Application.Caches;
using YA.WebClient.Application.States;
using YA.WebClient.Application.Services;
using YA.WebClient.Constants;
using YA.WebClient.Extensions;
using YA.WebClient.Infrastructure.Authentication;
using YA.WebClient.Infrastructure.HttpContext;
using YA.WebClient.Infrastructure.Repositories;
using YA.WebClient.Infrastructure.Services;
using YA.WebClient.Options;

[assembly: CLSCompliant(false)]
namespace YA.WebClient;

enum OsPlatform
{
    Unknown = 0,
    Windows = 1,
    Linux = 2,
    OSX = 3
}

public static class Program
{
    internal static readonly string AppVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public static async Task Main(string[] args)
    {
#if DEBUG
        //дебагер не всегда успевает подключиться к браузеру
        await Task.Delay(5000);
#endif
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");

        //Auth0 - нужно указывать audience, иначе получаем только проприетарный код, а не JWT
        //как альтернатива - можно указать аудиторию по-умолчанию для всех приложений в настройках тенанта Auth0
        builder.Services.AddOidcAuthentication<CustomRemoteAuthenticationState, CustomUserAccount>(options =>
        {
            builder.Configuration.Bind(nameof(OauthOptions), options.ProviderOptions);

            options.ProviderOptions.ResponseType = "code";
            options.ProviderOptions.DefaultScopes.Clear();
            options.ProviderOptions.DefaultScopes.Add("openid");
            options.ProviderOptions.DefaultScopes.Add("profile");
            options.ProviderOptions.DefaultScopes.Add("email");
        }).AddAccountClaimsPrincipalFactory<CustomRemoteAuthenticationState, CustomUserAccount, CustomUserFactory>();

        builder.Services.AddSingleton(s => s.GetService<IConfiguration>().GetSection(nameof(ApiOptions)).Get<ApiOptions>());
        builder.Services.AddSingleton(s => s.GetService<IConfiguration>().GetSection(nameof(VkontakteOptions)).Get<VkontakteOptions>());
        builder.Services.AddSingleton(s => s.GetService<IConfiguration>().GetSection(nameof(OauthOptions)).Get<OauthOptions>());

        builder.Services.AddScoped<IApiHttpContext, ApiHttpContext>();
        builder.Services.AddScoped<IApiRepository, ApiRepository>();
        builder.Services.AddScoped<IClientInfoService, ClientInfoService>();
        builder.Services.AddScoped<IVkTokenService, VkTokenService>();
        builder.Services.AddScoped<IRuntimeGeoDataService, IpWhoisRuntimeGeoData>();
        builder.Services.AddScoped<IEnvironmentContext, EnvironmentContext>();
        builder.Services.AddScoped<ISignService, YaSignService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IFileSaveService, FileSaver>();
        builder.Services.AddScoped<IUiUserSettingsService, UiUserSettingsService>();
        builder.Services.AddScoped(s => new VkOneTimeParsingTaskModal(s.GetService<IApiRepository>()));
        builder.Services.AddScoped(s => new VkPeriodicParsingTaskModal(s.GetService<IApiRepository>()));
        builder.Services.AddScoped(s => new TenantUserInvitationModal());
        builder.Services.AddScoped<IYaToaster, YaToaster>();
        builder.Services.AddScoped<IYaSessionStorage, YaSessionStorageService>();
        builder.Services.AddScoped<IWarmupDiService, WarmupDiService>();
        builder.Services.AddSingleton(s => new LoadingModalService());

        builder.Services.AddSingleton<IRuntimeState, RuntimeState>();
        builder.Services.AddSingleton<IVkOauthCodesState, VkOauthCodesState>();
        builder.Services.AddSingleton<IModalUserWarningState, ModalUserWarningState>();
        builder.Services.AddSingleton<IPageUserWarningState, PageUserWarningState>();
        builder.Services.AddSingleton<INotificationsPanelState, NotificationsPanelState>();
        builder.Services.AddSingleton<IThemeOptionsState, ThemeOptionsState>();

        //главный АПИ-клиент с отображением полосы загрузки страницы
        builder.Services
            .AddHttpClient("foreground", (sp, client) =>
            {
                client.BaseAddress = new Uri(sp.GetRequiredService<ApiOptions>().Endpoint);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(General.DefaultMediaType));
                client.DefaultRequestHeaders.Add("x-client-version", AppVersion);
                client.EnableIntercept(sp);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("foreground"));

        //фоновый АПИ-клиент
        builder.Services
            .AddHttpClient("background", (sp, client) =>
            {
                client.BaseAddress = new Uri(sp.GetRequiredService<ApiOptions>().Endpoint);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(General.DefaultMediaType));
                client.DefaultRequestHeaders.Add("x-client-version", AppVersion);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("background"));

        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddLoadingBar(options =>
        {
            options.LoadingBarColor = "#f66d45";
        });
        builder.UseLoadingBar();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddBlazoredSessionStorage();
        builder.Services.AddToaster(config =>
        {
            config.PositionClass = "custom-toast-top-right";
            config.PreventDuplicates = false;
            config.MaxDisplayedToasts = 5;
            config.NewestOnTop = true;
            config.MaximumOpacity = 100;
            config.ShowTransitionDuration = 100;
            config.VisibleStateDuration = 3000;
            config.HideTransitionDuration = 1000;
            config.ShowProgressBar = false;
            config.ShowCloseIcon = false;
            config.EscapeHtml = false;
            config.IconClasses = new ToastIconClasses { Success = "custom-toast-success" };
            config.ToastTitleClass = "custom-toast-title d-inline-block";
            config.ToastMessageClass = "custom-toast-message d-block";
        });
        builder.Services.AddPageVisibility();
        builder.Services
          .AddBlazorise(options =>
          {
              options.ChangeTextOnKeyPress = true;
          })
          .AddBootstrapProviders()
          .AddFontAwesomeIcons();

        WebAssemblyHost host = builder.Build();

        AuthenticationStateProvider authStateProvider = host.Services.GetRequiredService<AuthenticationStateProvider>();
        authStateProvider.AuthenticationStateChanged += async (task) =>
        {
            IJSRuntime js = host.Services.GetRequiredService<IJSRuntime>();
            IClientInfoService clientInfoService = host.Services.GetRequiredService<IClientInfoService>();
            AuthenticationState state = await task;
            string uri = host.Services.GetRequiredService<NavigationManager>().Uri;

            if ((bool)state.User?.Identity?.IsAuthenticated)
            {
                await js.ConsoleLog("событие входа " + uri);
                await clientInfoService.PublishClientInfoAsync();
            }
            else
            {
                await js.ConsoleLog("событие выхода " + uri);
            }
        };

        await host.RunAsync();
    }
}
