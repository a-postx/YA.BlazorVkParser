using AutoMapper;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Sotsera.Blazor.Toaster;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Options;
using YA.WebClient.Application.Caches;
using YA.WebClient.Extensions;

namespace YA.WebClient.Application.Services
{
    /// <summary>
    /// Сервис прогрева зависимостей.
    /// </summary>
    public class WarmupDiService : IWarmupDiService
    {
        public WarmupDiService(IServiceProvider serviceProvider, IJSRuntime js)
        {
            _js = js ?? throw new ArgumentNullException(nameof(js));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly IJSRuntime _js;
        private bool _finished;

        public async Task WarmUpAllAsync()
        {
            if (!_finished)
            {
                await _js.ConsoleLog("начинаем прогрев");

                var httpClientFactory = _serviceProvider.GetService(typeof(IHttpClientFactory));
                var configuration = _serviceProvider.GetService(typeof(IConfiguration));
                var apiHttpContext = _serviceProvider.GetService(typeof(IApiHttpContext));
                var apiRepo = _serviceProvider.GetService(typeof(IApiRepository));
                var tenantManager = _serviceProvider.GetService(typeof(IClientInfoService));
                var fileSaver = _serviceProvider.GetService(typeof(IFileSaveService));
                var modalUserWarning = _serviceProvider.GetService(typeof(IModalUserWarningState));
                var pageUserWarning = _serviceProvider.GetService(typeof(IPageUserWarningState));
                var runtimeState = _serviceProvider.GetService(typeof(IRuntimeState));
                var signManager = _serviceProvider.GetService(typeof(ISignService));
                var themeOptionsService = _serviceProvider.GetService(typeof(IThemeOptionsState));
                var toastFactory = _serviceProvider.GetService(typeof(IYaToaster));
                var uiUserSettings = _serviceProvider.GetService(typeof(IUiUserSettingsService));
                var vkOauthCodesContainer = _serviceProvider.GetService(typeof(IVkOauthCodesState));
                var vkTokenanager = _serviceProvider.GetService(typeof(IVkTokenService));
                var oneTimeModal = _serviceProvider.GetService(typeof(VkOneTimeParsingTaskModal));
                var periodicModal = _serviceProvider.GetService(typeof(VkPeriodicParsingTaskModal));
                var inviteTenantUserModal = _serviceProvider.GetService(typeof(TenantUserInvitationModal));
                var apiOptions = _serviceProvider.GetService(typeof(ApiOptions));
                var oauthOptions = _serviceProvider.GetService(typeof(OauthOptions));
                var vkOptions = _serviceProvider.GetService(typeof(VkontakteOptions));
                var jsRuntime = _serviceProvider.GetService(typeof(IJSRuntime));

                //Автомапер начиная с 6-й версии блокирует поток на ~2 сек, 5-я версия легковесная
                //если обновляться, то нужно выбрать незаметное для пользователя место прогрева
                var mapper = _serviceProvider.GetService(typeof(IMapper));
                var toaster = _serviceProvider.GetService(typeof(IToaster));

                var navigationManager = _serviceProvider.GetService(typeof(NavigationManager));
                var accessTokenProvider = _serviceProvider.GetService(typeof(IAccessTokenProvider));
                var authStateProvider = _serviceProvider.GetService(typeof(AuthenticationStateProvider));

                var sessionStorage = _serviceProvider.GetService(typeof(ISessionStorageService));
                var localStorage = _serviceProvider.GetService(typeof(ILocalStorageService));

                _finished = true;
                await _js.ConsoleLog("прогрев завершён");
            }
        }
    }
}
