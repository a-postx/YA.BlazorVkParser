namespace YA.WebClient.Application.Interfaces;

public interface ITokenService
{
    Task<Guid> GetTenantIdAsync();
    Task<string> GetTokenAsync();
}
