namespace YA.WebClient.Application.Models.Queries;

public class VkCodeResponse
{
#pragma warning disable IDE1006 // Naming Styles
    public string code { get; set; }
    public string error { get; set; }
    public string error_reason { get; set; }
    public string error_description { get; set; }
    public string state { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
