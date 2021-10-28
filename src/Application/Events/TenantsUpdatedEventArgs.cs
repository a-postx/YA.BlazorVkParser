using System;
using System.Collections.Generic;
using YA.WebClient.Application.Models.ViewModels;

namespace YA.WebClient.Application.Events
{
    public class TenantsUpdatedEventArgs : EventArgs
    {
        public ICollection<TenantVm> Tenants { get; set; }
    }
}
