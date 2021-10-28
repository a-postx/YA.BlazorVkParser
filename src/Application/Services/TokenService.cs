using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Application.Models.Internal;

namespace YA.WebClient.Application.Services
{
    public class TokenService : ITokenService
    {
        public TokenService(IAccessTokenProvider accessTokenProvider,
            AuthenticationStateProvider authStateProvider,
            IRuntimeState runtimeState,
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            _tokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
            _authStateProvider = authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
            _runtimeState = runtimeState ?? throw new ArgumentNullException(nameof(runtimeState));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IRuntimeState _runtimeState;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _js;

        public async Task<Guid> GetTenantIdAsync()
        {
            Guid result = Guid.Empty;

            string token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken jsonToken = handler.ReadToken(token);
                JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;

                Claim metadataClaim = tokenS.Claims.FirstOrDefault(e => e.Type == "http://yaapp.app_metadata");

                if (metadataClaim != null)
                {
                    AppMetadata appMetadata = JsonSerializer
                        .Deserialize<AppMetadata>(metadataClaim.Value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (!string.IsNullOrEmpty(appMetadata.Tid) && Guid.TryParse(appMetadata.Tid, out Guid tenantId))
                    {
                        return tenantId;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Достаёт токен доступа из подсистемы безопасности. При необходимости, происходит обновление
        /// токена у поставщика безопасности с помощью рефреш-токена или перенаправление (если требуется перезайти).
        /// </summary>
        /// <returns>Кешированный или обновлённый JWT-токен.</returns>
        public async Task<string> GetTokenAsync()
        {
            AuthenticationState state = await _authStateProvider.GetAuthenticationStateAsync();

            if (!(state?.User?.Identity?.IsAuthenticated ?? false))
            {
                //должно последовать автоперенаправление на логин через компонент RedirectToLogin
                return string.Empty;
            }

            AccessTokenResult tokenResult = await _tokenProvider.RequestAccessToken();

            if (tokenResult.Status == AccessTokenResultStatus.RequiresRedirect)
            {
                bool notCodePage = !_navigationManager.ToBaseRelativePath(_navigationManager.Uri)
                        .StartsWith("authentication/login-callback?code=", StringComparison.OrdinalIgnoreCase);

                if (notCodePage)
                {
                    bool redirectLaunched = _runtimeState.GetLoginRedirectLaunched();

                    if (!redirectLaunched)
                    {
                        _runtimeState.PutLoginRedirectLaunched(true);
                        //перенаправляем на логин вручную
                        _navigationManager.NavigateTo(tokenResult.RedirectUrl);
                    }
                }
            }
            else
            {
                if (tokenResult.TryGetToken(out AccessToken token))
                {
                    _runtimeState.PutLoginRedirectLaunched(false);

                    return token.Value;
                }
            }

            return string.Empty;
        }
    }
}
