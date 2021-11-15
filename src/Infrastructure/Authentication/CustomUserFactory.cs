using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;

namespace YA.WebClient.Infrastructure.Authentication;

public class CustomUserFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
{
    public CustomUserFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
    {
        _tokenProviderAccessor = accessor;
    }

    private readonly IAccessTokenProviderAccessor _tokenProviderAccessor;

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
    {
        ClaimsPrincipal user = await base.CreateUserAsync(account, options);

        ////if (user.Identity.IsAuthenticated)
        ////{
        ////    ClaimsIdentity identity = (ClaimsIdentity)user.Identity;

        ////    AccessTokenResult tokenResult = await _tokenProviderAccessor.TokenProvider.RequestAccessToken();

        ////    if (tokenResult.TryGetToken(out AccessToken token))
        ////    {
        ////        JwtSecurityTokenHandler handler = new();
        ////        SecurityToken jsonToken = handler.ReadToken(token.Value);
        ////        JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;

        ////        Claim metadataClaim = tokenS.Claims.FirstOrDefault(e => e.Type == "http://yaapp.app_metadata");

        ////        if (metadataClaim != null)
        ////        {
        ////            AppMetadata appMetadata = JsonSerializer
        ////                .Deserialize<AppMetadata>(metadataClaim.Value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        ////            if (!string.IsNullOrEmpty(appMetadata.Tid))
        ////            {
        ////                identity.AddClaim(new Claim("tid", appMetadata.Tid));
        ////            }
        ////        }
        ////    }
        ////}

        return user;
    }
}
