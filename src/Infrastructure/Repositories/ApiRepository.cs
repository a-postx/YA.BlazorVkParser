using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YA.WebClient.Application.Events;
using YA.WebClient.Application;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Application.Models.SaveModels;
using YA.WebClient.Application.Models.ViewModels;
using YA.WebClient.Options;
using System.Globalization;

namespace YA.WebClient.Infrastructure.Repositories
{
    public sealed class ApiRepository : IApiRepository
    {
        public ApiRepository(ApiOptions config, IApiHttpContext httpContext, ITokenService tokenService)
        {
            _cfg = config ?? throw new ArgumentNullException(nameof(config));
            _ctx = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        private readonly ApiOptions _cfg;
        private readonly IApiHttpContext _ctx;
        private readonly ITokenService _tokenService;

        public event EventHandler<ApiCallCompletedEventArgs> ApiCallCompleted;

        #region Helpers
        private Uri GetFullUrlV1(string relativePath)
        {
            string baseUriS = _cfg.Endpoint;
            baseUriS = baseUriS.EndsWith('/') ? baseUriS[0..^1] : baseUriS;

            string fullPath = AddApiVersion1(baseUriS, relativePath);

            Uri uri = new Uri(fullPath);

            return uri;
        }

        private Uri GetFullPaginationUrlV1(Uri linkUri)
        {
            string baseUriS = _cfg.Endpoint;
            baseUriS = baseUriS.EndsWith('/') ? baseUriS[0..^1] : baseUriS;
            baseUriS += $"/{_cfg.vk}";

            Uri baseUri = new Uri(baseUriS);
            Uri paramsUri = baseUri.MakeRelativeUri(linkUri);

            if (!paramsUri.Query.Contains("api-version", StringComparison.Ordinal))
            {
                if (paramsUri.Query.Contains('?', StringComparison.Ordinal))
                {
                    Uri uri = new Uri($"{baseUriS}{linkUri.PathAndQuery}&api-version=1.0");
                    return uri;
                }
                else
                {
                    Uri uri = new Uri($"{baseUriS}{linkUri.PathAndQuery}?api-version=1.0");
                    return uri;
                }
            }
            else
            {
                Uri uri = new Uri($"{baseUriS}{paramsUri.PathAndQuery}");
                return uri;
            }
        }

        private string AddApiVersion1(string uri, string path)
        {
            if (!path.Contains("api-version", StringComparison.Ordinal))
            {
                if (path.Contains('?', StringComparison.Ordinal))
                {
                    return $"{uri}{path}&api-version=1.0";
                }
                else
                {
                    return $"{uri}{path}?api-version=1.0";
                }
            }
            else
            {
                return $"{uri}{path}";
            }
        }

        private void PublishApiCallCompletedEvent<T>(ApiCommandResult<T> result) where T : class
        {
            ApiCallCompleted?.Invoke(this, new ApiCallCompletedEventArgs {
                Status = result.Status,
                Error = result.ErrorText,
                RequestId = result.RequestId
            });
        }
        #endregion

        #region User
        public async Task<ApiCommandResult<UserVm>> CreateUser(UserRegistrationInfoSm accessInfo)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/me");
            ApiCommandResult<UserVm> result = await _ctx
                .PostAsync<UserVm>(url, accessInfo, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<UserVm>> GetCurrentUser(bool background = true)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/me");
            ApiCommandResult<UserVm> result = await _ctx
                .GetAsync<UserVm>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: background);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<UserVm>> UpdateCurrentUser(JsonPatchDocument<UserSm> patch, bool background = true)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/me");
            ApiCommandResult<UserVm> result = await _ctx
                .PatchAsync<UserVm>(url, patch, _cfg.DefaultTimeoutMsec, useBackgroundClient: background);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<string>> SwitchUserTenant(Guid targetTenantId, bool background = true)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/me/switchtenant?targetTenantId={targetTenantId}");
            ApiCommandResult<string> result = await _ctx
                .PostAsync<string>(url, null, _cfg.DefaultTimeoutMsec, useBackgroundClient: background);

            return result;
        }
        #endregion

        #region Tenant
        public async Task<ApiCommandResult<TenantVm>> GetCurrentTenant(bool background = true)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenants");
            ApiCommandResult<TenantVm> result = await _ctx
                .GetAsync<TenantVm>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: background);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<TenantVm>> UpdateCurrentTenant(JsonPatchDocument<TenantSm> patch, bool background = true)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenants");
            ApiCommandResult<TenantVm> result = await _ctx.PatchAsync<TenantVm>(url, patch, _cfg.DefaultTimeoutMsec, useBackgroundClient: background);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        #region TenantInvitations
        public async Task<ApiCommandResult<InvitationVm>> CreateInvitation(InvitationSm invite)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenantinvitations");
            ApiCommandResult<InvitationVm> result = await _ctx.PostAsync<InvitationVm>(url, invite, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<InvitationVm>> GetInvitation(Guid yaTenantInvitationId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenantinvitations/{yaTenantInvitationId}");
            ApiCommandResult<InvitationVm> result = await _ctx
                .GetAsync<InvitationVm>(url, _cfg.DefaultTimeoutMsec, false, useBackgroundClient: true);

            return result;
        }

        public async Task<ApiCommandResult<string>> DeleteInvitation(Guid yaTenantInvitationId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenantinvitations/{yaTenantInvitationId}");
            ApiCommandResult<string> result = await _ctx.DeleteAsync<string>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        #region TenantMemberships
        public async Task<ApiCommandResult<MembershipVm>> CreateMembership(Guid token)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenantmemberships?token={token}");
            ApiCommandResult<MembershipVm> result = await _ctx.PostAsync<MembershipVm>(url, null, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            return result;
        }

        public async Task<ApiCommandResult<string>> DeleteMembership(Guid yaTenantMembershipId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/tenantmemberships/{yaTenantMembershipId}");
            ApiCommandResult<string> result = await _ctx.DeleteAsync<string>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        public Task<ApiCommandResult<ClientInfoVm>> PublishClientInfo(ClientInfoSm clientInfoSm)
        {
            Uri url = GetFullUrlV1($"/{_cfg.user}/clientinfos");
            return _ctx.PostAsync<ClientInfoVm>(url, clientInfoSm, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);
        }

        #region VkAccessTokens
        public async Task<ApiCommandResult<VkAccessTokenVm>> GetVkAccessToken()
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkaccesstokens");
            ApiCommandResult<VkAccessTokenVm> result = await _ctx.GetAsync<VkAccessTokenVm>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            return result;
        }

        public Task<ApiCommandResult<VkAccessTokenVm>> RequestAndCreateVkAccessToken(long vkAppId, string redirectAddress, string code)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkaccesstokens?vkappid={vkAppId}&redirectAddress={redirectAddress}&code={code}");
            return _ctx.PostAsync<VkAccessTokenVm>(url, null, _cfg.DefaultTimeoutMsec);
        }

        public async Task<ApiCommandResult<string>> DeleteVkAccessToken()
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkaccesstokens");
            ApiCommandResult<string> result = await _ctx.DeleteAsync<string>(url, _cfg.DefaultTimeoutMsec);

            return result;
        }
        #endregion

        public async Task<ApiCommandResult<ValidatedVkParsingTaskModalTm>> ValidateVkParsingTaskModal(VkParsingTaskModalTm parsingSourcesSm)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/parsingtaskmodal");
            ApiCommandResult<ValidatedVkParsingTaskModalTm> result = await _ctx
                .PostAsync<ValidatedVkParsingTaskModalTm>(url, parsingSourcesSm, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<List<VkAdsAccountVm>>> GetVkAdsAccounts()
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkadsaccounts");
            ApiCommandResult<List<VkAdsAccountVm>> result = await _ctx
                .GetAsync<List<VkAdsAccountVm>>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<List<VkAdsTargetGroupVm>>> GetVkAdsTargetGroups(long adsAccountId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkadstargetgroups?adsaccountid={adsAccountId}");
            ApiCommandResult<List<VkAdsTargetGroupVm>> result = await _ctx
                .GetAsync<List<VkAdsTargetGroupVm>>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        #region CommunitySearchFindings
        public async Task<ApiCommandResult<PaginatedResultVm<VkCommunityFindingVm>>> GetVkCommunitySearchFindings(Guid yaVkParsingTaskId, int pageNumber, int pageSize, string sortKey, string sortDirection)
        {
            string urlString = $"/{_cfg.vk}/communitysearchfindings?yaVkParsingTaskId={yaVkParsingTaskId}&pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(sortKey) && !string.IsNullOrEmpty(sortDirection))
            {
                urlString += $"&sort={sortKey}&sortDirection={sortDirection}";
            }

            Uri url = GetFullUrlV1(urlString);
            ApiCommandResult<PaginatedResultVm<VkCommunityFindingVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkCommunityFindingVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        #region VK OneTime Parsing Tasks
        public async Task<ApiCommandResult<VkOneTimeParsingTaskVm>> CreateVkParsingTask(VkOneTimeParsingTaskSm parsingTaskSm)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/onetimeparsingtasks");
            ApiCommandResult<VkOneTimeParsingTaskVm> result = await _ctx
                .PostAsync<VkOneTimeParsingTaskVm>(url, parsingTaskSm, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<VkOneTimeParsingTaskVm>> GetVkOneTimeParsingTask(Guid yaVkParsingTaskId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/onetimeparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<VkOneTimeParsingTaskVm> result = await _ctx
                .GetAsync<VkOneTimeParsingTaskVm>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> GetVkOneTimeParsingTasks(int pageSize, string titleSearch)
        {
            string urlString = $"/{_cfg.vk}/onetimeparsingtasks?first={pageSize}";

            if (!string.IsNullOrWhiteSpace(titleSearch))
            {
                urlString += $"&title={titleSearch}";
            }

            Uri url = GetFullUrlV1(urlString);

            ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkOneTimeParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> GetVkOneTimeParsingTasks(Uri uri)
        {
            Uri url = GetFullPaginationUrlV1(uri);
            ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkOneTimeParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> RefreshVkOneTimeParsingTasks(int pageSize, string titleSearch)
        {
            string urlString = $"/{_cfg.vk}/onetimeparsingtasks?first={pageSize}";

            if (!string.IsNullOrWhiteSpace(titleSearch))
            {
                urlString += $"&title={titleSearch}";
            }

            Uri url = GetFullUrlV1(urlString);

            ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkOneTimeParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> RefreshVkOneTimeParsingTasks(Uri uri)
        {
            Uri url = GetFullPaginationUrlV1(uri);
            ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkOneTimeParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<Uri> GetVkOneTimeParsingTaskResultDownloadLink(Guid yaVkParsingTaskId, string resultType)
        {
            string accessToken = await _tokenService.GetTokenAsync();

            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            Uri url = GetFullUrlV1($"/{_cfg.vk}/onetimeparsingtasks/{yaVkParsingTaskId}/download?type={resultType}&at={accessToken}");
            return url;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkQuickParsingTaskVm>>> GetQuickSearchVkOneTimeParsingTasks(int pageSize, string titleSearch)
        {
            string urlString = $"/{_cfg.vk}/onetimeparsingtasks/quicksearch?first={pageSize}";

            if (!string.IsNullOrWhiteSpace(titleSearch))
            {
                urlString += $"&title={titleSearch}";
            }

            Uri url = GetFullUrlV1(urlString);

            ApiCommandResult<PaginatedResultVm<VkQuickParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkQuickParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<ICollection<VkQuickParsingTaskVm>>> GetVkOneTimeParsingTasks(ICollection<Guid> ids)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CultureInfo.InvariantCulture, $"/{_cfg.vk}/onetimeparsingtasks/list");
            bool first = true;

            foreach (Guid id in ids)
            {
                string queryParam;

                if (first)
                {
                    queryParam = $"?ids={id}";
                    first = false;
                }
                else
                {
                    queryParam = $"&ids={id}";
                }

                sb.Append(queryParam);
            }

            string routeAndQuery = sb.ToString();

            Uri url = GetFullUrlV1(routeAndQuery);

            ApiCommandResult<ICollection<VkQuickParsingTaskVm>> result = await _ctx
                .GetAsync<ICollection<VkQuickParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            if (result.Status != Application.Enums.ApiCommandStatus.NotFound)
            {
                PublishApiCallCompletedEvent(result);
            }

            return result;
        }

        public async Task<ApiCommandResult<VkOneTimeParsingTaskVm>> UpdateVkOneTimeParsingTask(Guid yaVkParsingTaskId, JsonPatchDocument<VkOneTimeParsingTaskSm> patch)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/onetimeparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<VkOneTimeParsingTaskVm> result = await _ctx
                .PatchAsync<VkOneTimeParsingTaskVm>(url, patch, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<string>> DeleteVkOneTimeParsingTask(Guid yaVkParsingTaskId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/onetimeparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<string> result = await _ctx.DeleteAsync<string>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        #region VK Periodic Parsing Tasks
        public async Task<ApiCommandResult<VkPeriodicParsingTaskVm>> GetVkPeriodicParsingTask(Guid yaVkParsingTaskId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<VkPeriodicParsingTaskVm> result = await _ctx
                .GetAsync<VkPeriodicParsingTaskVm>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> GetVkPeriodicParsingTasks(int pageSize)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks?first={pageSize}");
            ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkPeriodicParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);
            
            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> GetVkPeriodicParsingTasks(Uri uri)
        {
            Uri url = GetFullPaginationUrlV1(uri);
            ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkPeriodicParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> RefreshVkPeriodicParsingTasks(int pageSize)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks?first={pageSize}");
            ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkPeriodicParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);
            
            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> RefreshVkPeriodicParsingTasks(Uri uri)
        {
            Uri url = GetFullPaginationUrlV1(uri);
            ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>> result = await _ctx
                .GetAsync<PaginatedResultVm<VkPeriodicParsingTaskVm>>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<long>> GetVkPeriodicParsingTasksCount()
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks/count");
            ApiCommandResult<long> result = await _ctx
                .GetAsync<long>(url, _cfg.DefaultTimeoutMsec, useBackgroundClient: true);

            return result;
        }

        public async Task<Uri> GetVkPeriodicParsingTaskResultDownloadLink(Guid yaVkParsingTaskId, string resultType)
        {
            string accessToken = await _tokenService.GetTokenAsync();

            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks/{yaVkParsingTaskId}/download?type={resultType}&at={accessToken}");
            return url;
        }

        public async Task<ApiCommandResult<VkPeriodicParsingTaskVm>> UpdateVkPeriodicParsingTask(Guid yaVkParsingTaskId, JsonPatchDocument<VkPeriodicParsingTaskSm> patch)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<VkPeriodicParsingTaskVm> result = await _ctx
                .PatchAsync<VkPeriodicParsingTaskVm>(url, patch, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }

        public async Task<ApiCommandResult<string>> DeleteVkPeriodicParsingTask(Guid yaVkParsingTaskId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/periodicparsingtasks/{yaVkParsingTaskId}");
            ApiCommandResult<string> result = await _ctx.DeleteAsync<string>(url, _cfg.DefaultTimeoutMsec);

            PublishApiCallCompletedEvent(result);

            return result;
        }
        #endregion

        #region VK Communities
        public Task<ApiCommandResult<PaginatedResultVm<VkCommunityCreateVm>>> GetVkCommunities()
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkcommunities");
            return _ctx.GetAsync<PaginatedResultVm<VkCommunityCreateVm>>(url, _cfg.DefaultTimeoutMsec);
        }
        
        public Task<ApiCommandResult<PaginatedResultVm<VkCommunityCreateVm>>> GetVkCommunities(Uri uri)
        {
            Uri url = GetFullPaginationUrlV1(uri);
            return _ctx.GetAsync<PaginatedResultVm<VkCommunityCreateVm>>(url, _cfg.DefaultTimeoutMsec);
        }

        public Task<ApiCommandResult<VkCommunityCreateVm>> CreateVkMonitoringCommunity(VkCommunitySm vkCommunitySm)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkcommunities");
            return _ctx.PostAsync<VkCommunityCreateVm>(url, vkCommunitySm, _cfg.DefaultTimeoutMsec);
        }

        public Task<ApiCommandResult<VkCommunityProfileIdsVm>> GetVkCommunityProfileIds(Guid vkCommunityId)
        {
            Uri url = GetFullUrlV1($"/{_cfg.vk}/vkcommunities/{vkCommunityId}/profileids");
            return _ctx.GetAsync<VkCommunityProfileIdsVm>(url, _cfg.DefaultTimeoutMsec);
        }
        #endregion
    }
}
