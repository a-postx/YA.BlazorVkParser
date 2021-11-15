namespace YA.WebClient.Application.Interfaces;

public interface IRuntimeState
{
    event EventHandler LoginRedirectLaunchedUpdated;
    event EventHandler UserUpdated;
    event EventHandler<TenantUpdatedEventArgs> TenantUpdated;
    event EventHandler<MembershipUpdatedEventArgs> MembershipUpdated;
    event EventHandler<TenantsUpdatedEventArgs> TenantsUpdated;
    event EventHandler PricingTierUpdated;

    bool GetLoginRedirectLaunched();
    void PutLoginRedirectLaunched(bool value);

    UserVm GetUser();
    void PutUser(UserVm user);
    void RemoveUser();

    TenantVm GetTenant();
    void PutTenant(TenantVm tenant);
    void RemoveTenant();

    ICollection<TenantVm> GetTenants();
    void PutTenants(ICollection<TenantVm> tenants);
    void RemoveTenants();

    MembershipVm GetMembership();
    void PutMembership(MembershipVm membership);
    void RemoveMembership();

    PricingTierVm GetPricingTier();
    void PutPricingTier(PricingTierVm pricingTier);
    void RemovePricingTier();

    VkAccessTokenVm GetVkAccessToken();
    void AddVkAccessToken(VkAccessTokenVm token);
    void RemoveVkAccessToken();
}
