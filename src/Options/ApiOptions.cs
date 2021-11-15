using System.Text.Json;

namespace YA.WebClient.Options;

public class ApiOptions
{
    public string Endpoint { get; set; }
    public int DefaultTimeoutMsec { get; set; }
    public JsonSerializerOptions SerializerOptions { get; set; } = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

#pragma warning disable IDE1006 // имена путей в нижнем регистре для апи-вызовов
    public string user { get; set; } = nameof(user);
    public string vk { get; set; } = nameof(vk);
#pragma warning restore IDE1006 // Naming Styles
}
