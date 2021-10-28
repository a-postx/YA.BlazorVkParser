using System.Threading;
using System.Threading.Tasks;

namespace YA.WebClient.Application.Interfaces
{
    public interface IUiUserSettingsService
    {
        Task<int> GetParsingTasksPaginationPageSizeAsync(CancellationToken cancellationToken);
        Task SetParsingTasksPaginationPageSizeAsync(int pageSize, CancellationToken cancellationToken);

        Task<int> GetParsingResultCommunitiesPaginationPageSizeAsync(CancellationToken cancellationToken);
        Task SetParsingResultCommunitiesPaginationPageSizeAsync(int pageSize, CancellationToken cancellationToken);

        Task SetVkCommunityAlertDismissedAsync(bool dismissed, CancellationToken cancellationToken);
        Task<bool> GetVkCommunityAlertDismissedAsync(CancellationToken cancellationToken);
    }
}
