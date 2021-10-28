using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Options;

namespace YA.WebClient.Application.Services
{
    public class YaSignService : ISignService
    {
        public YaSignService(SignOutSessionStateManager stateManager,
            NavigationManager navigationManager,
            OauthOptions oauthOptions,
            IYaSessionStorage sessionStorage)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _oauthOptions = oauthOptions ?? throw new ArgumentNullException(nameof(oauthOptions));
            _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
        }

        private readonly SignOutSessionStateManager _stateManager;
        private readonly NavigationManager _navigationManager;
        private readonly OauthOptions _oauthOptions;
        private readonly IYaSessionStorage _sessionStorage;

        public void BeginLogin(string returnAddress = "")
        {
            string navigationUrl = "authentication/login";

            if (!string.IsNullOrEmpty(returnAddress))
            {
                string escapedReturnUrl = Uri.EscapeDataString(returnAddress);
                navigationUrl += $"?returnUrl={escapedReturnUrl}";
            }

            _navigationManager.NavigateTo(navigationUrl);
        }

        public async Task BeginLogoutAsync(string returnAddress = "", CancellationToken cancellationToken = default)
        {
            await _stateManager.SetSignOutState();

            await _sessionStorage.RemoveCurrentOidcTokensAsync(cancellationToken);
            
            SignOutOnAuth0(returnAddress);
        }

        private void SignOutOnAuth0(string returnUrl)
        {
            string clientId = _oauthOptions.ClientId;
            string authority = _oauthOptions.Authority;

            string navigationUrl = $"{authority}/v2/logout?client_id={clientId}";

            if (!string.IsNullOrEmpty(returnUrl))
            {
                string escapedReturnUrl = Uri.EscapeDataString(returnUrl);
                navigationUrl += $"&returnUrl={escapedReturnUrl}";
            }

            _navigationManager.NavigateTo(navigationUrl);
        }
    }
}
