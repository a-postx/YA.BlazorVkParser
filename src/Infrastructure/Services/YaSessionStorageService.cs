using Blazored.SessionStorage;
using YA.WebClient.Options;

namespace YA.WebClient.Infrastructure.Services;

/// <summary>
/// Абстракция для работы с сессионным хранилищем браузера
/// </summary>
public class YaSessionStorageService : IYaSessionStorage
{
    public YaSessionStorageService(ISessionStorageService sessionStorageService,
        OauthOptions oauthOptions,
        IJSRuntime jsRuntime)
    {
        _sessionStorageService = sessionStorageService ?? throw new ArgumentNullException(nameof(sessionStorageService));
        _oauthOptions = oauthOptions ?? throw new ArgumentNullException(nameof(oauthOptions));
        _js = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }
    private readonly ISessionStorageService _sessionStorageService;
    private readonly OauthOptions _oauthOptions;
    private readonly IJSRuntime _js;

    /// <summary>
    /// Удалить кешированный токен
    /// </summary>
    public async Task RemoveCurrentOidcTokensAsync(CancellationToken cancellationToken)
    {
        string oidcSessionStorageCacheKey = $"oidc.user:{_oauthOptions.Authority}:{_oauthOptions.ClientId}";

        if (await _sessionStorageService.ContainKeyAsync(oidcSessionStorageCacheKey, cancellationToken))
        {
            await _sessionStorageService.RemoveItemAsync(oidcSessionStorageCacheKey, cancellationToken);
        }
    }

    /// <summary>
    /// Добавить идентификатор приглашения (или заменить, если он уже существует)
    /// </summary>
    public async Task AddOrReplaceTenantInvitationAsync(Guid tenantInvitationId, CancellationToken cancellationToken)
    {
        string tenantInvitationSessionStorageKey = $"registration.jointeamtoken";

        if (await _sessionStorageService.ContainKeyAsync(tenantInvitationSessionStorageKey, cancellationToken))
        {
            await _sessionStorageService.RemoveItemAsync(tenantInvitationSessionStorageKey, cancellationToken);
        }

        await _sessionStorageService
            .SetItemAsync(tenantInvitationSessionStorageKey, tenantInvitationId, cancellationToken);
    }

    /// <summary>
    /// Получить идентификатор приглашения (если он есть)
    /// </summary>
    public async Task<Guid> GetTenantInvitationAsync(CancellationToken cancellationToken)
    {
        string tenantInvitationSessionStorageKey = $"registration.jointeamtoken";

        if (await _sessionStorageService.ContainKeyAsync(tenantInvitationSessionStorageKey, cancellationToken))
        {
            Guid invitationId = await _sessionStorageService
                .GetItemAsync<Guid>(tenantInvitationSessionStorageKey, cancellationToken);

            return invitationId;
        }
        else
        {
            return Guid.Empty;
        }
    }

    /// <summary>
    /// Удалить идентификатор приглашения (если он есть)
    /// </summary>
    public async Task RemoveTenantInvitationAsync(CancellationToken cancellationToken)
    {
        string tenantInvitationSessionStorageKey = $"registration.jointeamtoken";

        if (await _sessionStorageService.ContainKeyAsync(tenantInvitationSessionStorageKey, cancellationToken))
        {
            await _sessionStorageService.RemoveItemAsync(tenantInvitationSessionStorageKey, cancellationToken);
        }
    }
}
