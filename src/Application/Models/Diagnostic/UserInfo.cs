namespace YA.WebClient.Application.Models.Diagnostic;

public class UserInfo
{
    public string Username { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string TenantId { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}
