using System.Threading.Tasks;

namespace YA.WebClient.Application.Interfaces
{
    public interface IWarmupDiService
    {
        Task WarmUpAllAsync();
    }
}
