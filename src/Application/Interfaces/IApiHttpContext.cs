using System;
using System.Threading.Tasks;

namespace YA.WebClient.Application.Interfaces
{
    public interface IApiHttpContext
    {
        event EventHandler<EventArgs> TenantSelectionRequired;

        Task<ApiCommandResult<T>> PostAsync<T>(Uri url, object data, int timeout, bool auth = true, bool useBackgroundClient = false);
        Task<ApiCommandResult<T>> GetAsync<T>(Uri url, int timeout, bool auth = true, bool useBackgroundClient = false);
        Task<ApiCommandResult<T>> DeleteAsync<T>(Uri url, int timeout, bool auth = true);
        Task<ApiCommandResult<T>> PatchAsync<T>(Uri url, object data, int timeout, bool auth = true, bool useBackgroundClient = false);
        Task<ApiCommandResult<T>> PutAsync<T>(Uri url, object data, int timeout, bool auth = true);
    }
}
