using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YA.WebClient.Infrastructure.Authentication
{
    public class CustomRemoteAuthenticationState : RemoteAuthenticationState
    {
        public string PartnerLink { get; set; }
    }
}
