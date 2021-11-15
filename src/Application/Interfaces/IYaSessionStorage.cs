namespace YA.WebClient.Application.Interfaces;

public interface IYaSessionStorage
{
    Task AddOrReplaceTenantInvitationAsync(Guid tenantInvitationId, CancellationToken cancellationToken = default);
    Task<Guid> GetTenantInvitationAsync(CancellationToken cancellationToken = default);
    Task RemoveTenantInvitationAsync(CancellationToken cancellationToken = default);

    Task RemoveCurrentOidcTokensAsync(CancellationToken cancellationToken = default);
}
