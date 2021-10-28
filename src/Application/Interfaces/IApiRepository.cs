using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YA.WebClient.Application.Events;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Application.Models.SaveModels;
using YA.WebClient.Application.Models.ViewModels;

namespace YA.WebClient.Application.Interfaces
{
    public interface IApiRepository
    {
        event EventHandler<ApiCallCompletedEventArgs> ApiCallCompleted;

        Task<ApiCommandResult<UserVm>> CreateUser(UserRegistrationInfoSm accessInfo);
        Task<ApiCommandResult<UserVm>> GetCurrentUser(bool background = true);
        Task<ApiCommandResult<UserVm>> UpdateCurrentUser(JsonPatchDocument<UserSm> patch, bool background = true);
        Task<ApiCommandResult<string>> SwitchUserTenant(Guid targetTenantId, bool background = true);

        Task<ApiCommandResult<TenantVm>> GetCurrentTenant(bool background = true);
        Task<ApiCommandResult<TenantVm>> UpdateCurrentTenant(JsonPatchDocument<TenantSm> patch, bool background = true);
        
        Task<ApiCommandResult<InvitationVm>> CreateInvitation(InvitationSm invite);
        Task<ApiCommandResult<InvitationVm>> GetInvitation(Guid yaTenantInvitationId);
        Task<ApiCommandResult<string>> DeleteInvitation(Guid yaTenantInvitationId);

        Task<ApiCommandResult<MembershipVm>> CreateMembership(Guid token);
        Task<ApiCommandResult<string>> DeleteMembership(Guid yaTenantMembershipId);

        Task<ApiCommandResult<VkAccessTokenVm>> GetVkAccessToken();
        Task<ApiCommandResult<VkAccessTokenVm>> RequestAndCreateVkAccessToken(long vkAppId, string redirectAddress, string code);
        Task<ApiCommandResult<string>> DeleteVkAccessToken();

        Task<ApiCommandResult<ValidatedVkParsingTaskModalTm>> ValidateVkParsingTaskModal(VkParsingTaskModalTm parsingSourcesSm);

        Task<ApiCommandResult<List<VkAdsAccountVm>>> GetVkAdsAccounts();
        Task<ApiCommandResult<List<VkAdsTargetGroupVm>>> GetVkAdsTargetGroups(long adsAccountId);

        Task<ApiCommandResult<VkOneTimeParsingTaskVm>> CreateVkParsingTask(VkOneTimeParsingTaskSm parsingTaskSm);
        Task<ApiCommandResult<VkOneTimeParsingTaskVm>> GetVkOneTimeParsingTask(Guid yaVkParsingTaskId);
        Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> GetVkOneTimeParsingTasks(int pageSize, string titleSearch);
        Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> GetVkOneTimeParsingTasks(Uri uri);
        Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> RefreshVkOneTimeParsingTasks(int pageSize, string titleSearch);
        Task<ApiCommandResult<PaginatedResultVm<VkOneTimeParsingTaskVm>>> RefreshVkOneTimeParsingTasks(Uri uri);
        Task<ApiCommandResult<PaginatedResultVm<VkQuickParsingTaskVm>>> GetQuickSearchVkOneTimeParsingTasks(int pageSize, string titleSearch);
        Task<ApiCommandResult<ICollection<VkQuickParsingTaskVm>>> GetVkOneTimeParsingTasks(ICollection<Guid> ids);
        Task<Uri> GetVkOneTimeParsingTaskResultDownloadLink(Guid yaVkParsingTaskId, string resultType);
        Task<ApiCommandResult<VkOneTimeParsingTaskVm>> UpdateVkOneTimeParsingTask(Guid yaVkParsingTaskId, JsonPatchDocument<VkOneTimeParsingTaskSm> patch);
        Task<ApiCommandResult<string>> DeleteVkOneTimeParsingTask(Guid yaVkParsingTaskId);

        Task<ApiCommandResult<VkPeriodicParsingTaskVm>> GetVkPeriodicParsingTask(Guid yaVkParsingTaskId);
        Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> GetVkPeriodicParsingTasks(int pageSize);
        Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> GetVkPeriodicParsingTasks(Uri uri);
        Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> RefreshVkPeriodicParsingTasks(int pageSize);
        Task<ApiCommandResult<PaginatedResultVm<VkPeriodicParsingTaskVm>>> RefreshVkPeriodicParsingTasks(Uri uri);
        Task<ApiCommandResult<long>> GetVkPeriodicParsingTasksCount();
        Task<Uri> GetVkPeriodicParsingTaskResultDownloadLink(Guid yaVkParsingTaskId, string resultType);
        Task<ApiCommandResult<VkPeriodicParsingTaskVm>> UpdateVkPeriodicParsingTask(Guid yaVkParsingTaskId, JsonPatchDocument<VkPeriodicParsingTaskSm> patch);
        Task<ApiCommandResult<string>> DeleteVkPeriodicParsingTask(Guid yaVkParsingTaskId);

        Task<ApiCommandResult<PaginatedResultVm<VkCommunityFindingVm>>> GetVkCommunitySearchFindings(Guid yaVkParsingTaskId, int pageNumber, int pageSize, string sortKey, string sortDirection);
        
        Task<ApiCommandResult<ClientInfoVm>> PublishClientInfo(ClientInfoSm clientInfoSm);
    }
}
