using System.Threading.Tasks;
using YA.WebClient.Application.Models.Diagnostic;
using YA.WebClient.Application.Models.Dto;

namespace YA.WebClient.Application.Interfaces
{
    public interface IEnvironmentContext
    {
        Task<ClientInfo> GetClientInfoAsync();
        Task<DiagnosticInfo> GetDiagnosticInfoAsync();
    }
}
