namespace YA.WebClient.Application.Events;

public class TenantUpdatedEventArgs : EventArgs
{
    public TenantVm Tenant { get; set; }
}
