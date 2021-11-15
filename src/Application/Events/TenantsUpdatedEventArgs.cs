namespace YA.WebClient.Application.Events;

public class TenantsUpdatedEventArgs : EventArgs
{
    public ICollection<TenantVm> Tenants { get; set; }
}
