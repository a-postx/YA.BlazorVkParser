namespace YA.WebClient.Application.States;

/// <summary>
/// Хранилище состояния. При росте приложения переделать на Флюксор или подобное.
/// </summary>
public class RuntimeState : IRuntimeState
{
    public RuntimeState()
    {

    }

    public event EventHandler LoginRedirectLaunchedUpdated;
    public event EventHandler UserUpdated;
    public event EventHandler<TenantUpdatedEventArgs> TenantUpdated;
    public event EventHandler<MembershipUpdatedEventArgs> MembershipUpdated;
    public event EventHandler<TenantsUpdatedEventArgs> TenantsUpdated;
    public event EventHandler PricingTierUpdated;
    public event EventHandler VkAccessTokenUpdated;

    private bool _loginRedirectLaunched;
    private UserVm _user;
    private TenantVm _tenant;
    private MembershipVm _membership;
    private ICollection<TenantVm> _tenants;
    private PricingTierVm _pricingTier;
    private VkAccessTokenVm _vkAccessToken;

    private bool LoginRedirectLaunched
    {
        get
        {
            return _loginRedirectLaunched;
        }
        set
        {
            if (value != _loginRedirectLaunched)
            {
                _loginRedirectLaunched = value;
                NotifyLoginRedirectLaunchedUpdated();
            }
        }
    }

    private UserVm User
    {
        get
        {
            return _user;
        }

        set
        {
            if (value != _user)
            {
                _user = value;
                NotifyUserUpdated();
            }
        }
    }

    private TenantVm Tenant
    {
        get
        {
            return _tenant;
        }

        set
        {
            if (value != _tenant)
            {
                _tenant = value;
                NotifyTenantUpdated(value);
            }
        }
    }

    private MembershipVm Membership
    {
        get
        {
            return _membership;
        }

        set
        {
            if (value != _membership)
            {
                _membership = value;
                NotifyMembershipUpdated(value);
            }
        }
    }

    private ICollection<TenantVm> Tenants
    {
        get
        {
            return _tenants;
        }

        set
        {
            if (value != _tenants)
            {
                _tenants = value;
                NotifyTenantsUpdated(value);
            }
        }
    }

    private PricingTierVm PricingTier
    {
        get
        {
            return _pricingTier;
        }

        set
        {
            if (value != _pricingTier)
            {
                _pricingTier = value;
                NotifyPricingTierUpdated();
            }
        }
    }

    private VkAccessTokenVm VkAccessToken
    {
        get
        {
            return _vkAccessToken;
        }

        set
        {
            if (value != _vkAccessToken)
            {
                _vkAccessToken = value;
                NotifyVkAccessTokenUpdated();
            }
        }
    }

    #region LoginRedirect
    private void NotifyLoginRedirectLaunchedUpdated()
    {
        LoginRedirectLaunchedUpdated?.Invoke(this, null);
    }

    public bool GetLoginRedirectLaunched() => LoginRedirectLaunched;
    public void PutLoginRedirectLaunched(bool value) => LoginRedirectLaunched = value;
    #endregion

    #region Tenant
    private void NotifyTenantUpdated(TenantVm tenant)
    {
        TenantUpdated?.Invoke(this, new TenantUpdatedEventArgs { Tenant = tenant });
    }

    public TenantVm GetTenant() => Tenant;
    public void PutTenant(TenantVm tenant)
    {
        Tenant = tenant;
        ReplaceInList(tenant);
    }
    public void RemoveTenant()
    {
        RemoveFromList(Tenant);
        Tenant = null;
    }
    #endregion

    #region Tenants
    private void NotifyTenantsUpdated(ICollection<TenantVm> tenants)
    {
        TenantsUpdated?.Invoke(this, new TenantsUpdatedEventArgs { Tenants = tenants });
    }

    public ICollection<TenantVm> GetTenants() => Tenants;
    public void PutTenants(ICollection<TenantVm> tenants) => Tenants = tenants;
    private void ReplaceInList(TenantVm tenant)
    {
        TenantVm cachedTenant = Tenants?.Where(e => e.TenantId == tenant.TenantId).FirstOrDefault();

        if (cachedTenant is not null)
        {
            ICollection<TenantVm> newTenants = new List<TenantVm>();

            Tenants.ToList().ForEach(e =>
            {
                if (e.TenantId == tenant.TenantId)
                {
                    newTenants.Add(tenant);
                }
                else
                {
                    newTenants.Add(e);
                }
            });

            Tenants = newTenants;
        }
    }
    private void RemoveFromList(TenantVm tenant)
    {
        TenantVm cachedTenant = Tenants?.Where(e => e.TenantId == tenant.TenantId).FirstOrDefault();

        if (cachedTenant is not null)
        {
            ICollection<TenantVm> newTenants = new List<TenantVm>();

            Tenants.ToList().ForEach(e =>
            {
                if (e.TenantId != tenant.TenantId)
                {
                    newTenants.Add(e);
                }
            });

            Tenants = newTenants;
        }
    }
    public void RemoveTenants() => Tenants = null;
    #endregion

    #region User
    private void NotifyUserUpdated()
    {
        UserUpdated?.Invoke(this, null);
    }

    public UserVm GetUser() => User;
    public void PutUser(UserVm user) => User = user;
    public void RemoveUser() => User = null;
    #endregion

    #region PricingTier
    private void NotifyPricingTierUpdated()
    {
        PricingTierUpdated?.Invoke(this, null);
    }

    public PricingTierVm GetPricingTier() => PricingTier;
    public void PutPricingTier(PricingTierVm pricingTier) => PricingTier = pricingTier;
    public void RemovePricingTier() => PricingTier = null;
    #endregion

    #region Membership
    private void NotifyMembershipUpdated(MembershipVm membership)
    {
        MembershipUpdated?.Invoke(this, new MembershipUpdatedEventArgs { Membership = membership });
    }

    public MembershipVm GetMembership() => Membership;
    public void PutMembership(MembershipVm membership) => Membership = membership;
    public void RemoveMembership() => Membership = null;
    #endregion

    #region VkAccessToken
    private void NotifyVkAccessTokenUpdated()
    {
        VkAccessTokenUpdated?.Invoke(this, null);
    }

    public VkAccessTokenVm GetVkAccessToken() => VkAccessToken;
    public void AddVkAccessToken(VkAccessTokenVm token) => VkAccessToken = token;
    public void RemoveVkAccessToken() => VkAccessToken = null;
    #endregion
}
