using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YA.WebClient.Infrastructure.Authentication
{
    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("amr")]
        public string[] AuthenticationMethod { get; set; } = Array.Empty<string>();
    }
}
