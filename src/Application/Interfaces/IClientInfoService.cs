using System;
using System.Threading.Tasks;
using YA.WebClient.Application.Models.ViewModels;

namespace YA.WebClient.Application.Interfaces
{
    public interface IClientInfoService
    {
        Task<(ClientInfoVm, Guid)> PublishClientInfoAsync();
    }
}
