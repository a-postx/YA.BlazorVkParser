using Blazored.LocalStorage;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YA.WebClient.Application.Interfaces;

namespace YA.WebClient.Application.Services
{
    public class UiUserSettingsService : IUiUserSettingsService
    {
        public UiUserSettingsService(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }

        private readonly ILocalStorageService _storageService;

        private const string ParsingTasksPaginationPageSizeKey = "Ya.ParsingTasks.PaginationPageSize";
        private const string ParsingResultCommunitiesPaginationPageSizeKey = "Ya.ParsingResultCommunities.PaginationPageSize";
        private const string VkCommunityAlertDismissedKey = "Ya.ParsingTasks.VkCommunityAlertDismissed";

        public async Task SetParsingTasksPaginationPageSizeAsync(int pageSize, CancellationToken cancellationToken)
        {
            await _storageService
#pragma warning disable CA1305 // При указании провайдера выскакивает непонятная ошибка блейзора (js)
                .SetItemAsync(ParsingTasksPaginationPageSizeKey, pageSize.ToString(), cancellationToken);
#pragma warning restore CA1305 // Specify IFormatProvider
        }

        public async Task<int> GetParsingTasksPaginationPageSizeAsync(CancellationToken cancellationToken)
        {
            if (await _storageService.ContainKeyAsync(ParsingTasksPaginationPageSizeKey))
            {
                string savedValue = await _storageService
                    .GetItemAsync<string>(ParsingTasksPaginationPageSizeKey, cancellationToken);
                int result = JsonSerializer.Deserialize<int>(savedValue);

                return result;
            }
            else
            {
                return default;
            }
        }

        public async Task SetParsingResultCommunitiesPaginationPageSizeAsync(int pageSize, CancellationToken cancellationToken)
        {
            await _storageService
#pragma warning disable CA1305 // При указании провайдера выскакивает непонятная ошибка блейзора (js)
                .SetItemAsync(ParsingResultCommunitiesPaginationPageSizeKey, pageSize.ToString(), cancellationToken);
#pragma warning restore CA1305 // Specify IFormatProvider
        }

        public async Task<int> GetParsingResultCommunitiesPaginationPageSizeAsync(CancellationToken cancellationToken)
        {
            if (await _storageService.ContainKeyAsync(ParsingResultCommunitiesPaginationPageSizeKey))
            {
                string savedValue = await _storageService
                    .GetItemAsync<string>(ParsingResultCommunitiesPaginationPageSizeKey, cancellationToken);
                int result = JsonSerializer.Deserialize<int>(savedValue);

                return result;
            }
            else
            {
                return default;
            }
        }

        public async Task SetVkCommunityAlertDismissedAsync(bool dismissed, CancellationToken cancellationToken)
        {
            await _storageService
                .SetItemAsync(VkCommunityAlertDismissedKey, dismissed, cancellationToken);
        }

        public async Task<bool> GetVkCommunityAlertDismissedAsync(CancellationToken cancellationToken)
        {
            if (await _storageService.ContainKeyAsync(VkCommunityAlertDismissedKey))
            {
                string savedValue = await _storageService
                    .GetItemAsync<string>(VkCommunityAlertDismissedKey, cancellationToken);
                bool result = JsonSerializer.Deserialize<bool>(savedValue);

                return result;
            }
            else
            {
                return default;
            }
        }
    }
}
