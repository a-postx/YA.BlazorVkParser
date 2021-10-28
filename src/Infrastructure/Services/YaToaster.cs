using Sotsera.Blazor.Toaster;
using System;
using YA.WebClient.Application.Interfaces;

namespace YA.WebClient.Infrastructure.Services
{
    public class YaToaster : IYaToaster
    {
        public YaToaster(IToaster toaster)
        {
            _toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        private readonly IToaster _toaster;

        public void Success(string message)
        {
            _toaster.Success(CreateSuccessBody(message), null, conf => conf.EscapeHtml = false);
        }

        private string CreateSuccessBody(string message)
        {
            return $"<div><div style=\"float:left;font-size:1.25rem;color:#53ad57\"><i class=\"fas fa-check-circle\"></i></div><div class=\"pl-3\" style=\"overflow:hidden;font-weight:500;padding:4px 0\">{message}</div></div>";
        }
    }
}
