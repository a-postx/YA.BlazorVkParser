using System;
using System.Threading.Tasks;
using YA.WebClient.Application.Models.ViewModels;

namespace YA.WebClient.Application.Interfaces
{
    public interface IVkTokenService
    {
        Task GetVkCode(string redirectAddress);
        Task<(VkAccessTokenVm, string, Guid)> RequestAndSaveVkAccessToken(string code);
    }
}
