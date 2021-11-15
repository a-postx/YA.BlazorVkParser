using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace YA.WebClient.Infrastructure.Authentication;

public class CustomUserAccount : RemoteUserAccount
{
    [JsonPropertyName("amr")]
    public string[] AuthenticationMethod { get; set; } = Array.Empty<string>();
}
