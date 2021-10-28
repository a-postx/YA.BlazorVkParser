using System;
using YA.WebClient.Application.Enums;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Application.Models.SaveModels;

namespace YA.WebClient.Application.Caches
{
    public class VkOneTimeParsingTaskModal : ParsingTaskModal
    {
        public VkOneTimeParsingTaskModal(IApiRepository apiRepository) : base (apiRepository)
        {

        }

        public VkOneTimeParsingTaskSm CreateSm()
        {
            VkActivitySourceOptionsSm vkActivitySourceOptions = null;

            if (ProfilesResultActiveSourcePosts.HasValue && ProfilesResultActiveSourceDiscussions.HasValue)
            {
                vkActivitySourceOptions = new VkActivitySourceOptionsSm(
                    ProfilesResultActiveSourcePosts.Value,
                    ProfilesResultActiveSourceDiscussions.Value);
            }

            VkActivityTypeOptionsSm vkActivityTypeOptions = null;

            if (ProfilesResultActiveTypeLikes.HasValue)
            {
                vkActivityTypeOptions = new VkActivityTypeOptionsSm(
                ProfilesResultActiveTypeLikes.Value,
                ProfilesResultActiveTypeLikesInComments.Value,
                ProfilesResultActiveTypeComments.Value,
                ProfilesResultActiveTypeReposts.Value,
                ProfilesResultActiveTypePostAuthors.Value);
            }

            VkActiveProfilesOptionsSm activeProfilesOptionsSm = null;
            
            if (ProfilesResultActivePeriodStart.HasValue && ProfilesResultActivePeriodStart != DateTime.MinValue
                && ProfilesResultActivePeriodEnd.HasValue && ProfilesResultActivePeriodEnd != DateTime.MinValue)
            {
                activeProfilesOptionsSm = new VkActiveProfilesOptionsSm(
                    ProfilesResultActivePeriodStart.Value,
                    ProfilesResultActivePeriodEnd.Value,
                    ProfilesResultActiveActivityCountFrom.HasValue ? ProfilesResultActiveActivityCountFrom.Value : 1,
                    vkActivitySourceOptions,
                    vkActivityTypeOptions,
                    ProfilesResultActiveLimitWallPostsCount.HasValue ? ProfilesResultActiveLimitWallPostsCount.Value : true);
            }

            VkTopProfilesOptionsSm topProfilesOptionsSm = null;

            if (ProfilesResultTopType != VkParsingTaskResultProfileTopType.Unknown
                && TopCommunitiesCount.HasValue && TopCommunitiesCount.Value > 0)
            {
                topProfilesOptionsSm = new VkTopProfilesOptionsSm(
                    ProfilesResultTopType,
                    TopCommunitiesCount.Value);
            }

            VkGroupIntersectionProfilesOptionsSm groupIntersectOptionsSm = null;

            if (ProfilesResultGiCountFrom.HasValue && ProfilesResultGiCountFrom.Value > 0 
                && ProfilesResultGiCountTo.HasValue && ProfilesResultGiCountTo.Value > 0)
            {
                groupIntersectOptionsSm = new VkGroupIntersectionProfilesOptionsSm(
                    ProfilesResultGiCountFrom.Value,
                    ProfilesResultGiCountTo.Value);
            }

            VkFriendsProfilesOptionsSm friendsOptionsSm = null;

            if (ProfilesResultFriendsGetFriends.HasValue && ProfilesResultFriendsGetFollowers.HasValue
                && ProfilesResultFriendsGetPeopleSubscriptions.HasValue)
            {
                friendsOptionsSm = new VkFriendsProfilesOptionsSm(
                    ProfilesResultFriendsGetFriends.Value,
                    ProfilesResultFriendsGetFollowers.Value,
                    ProfilesResultFriendsGetPeopleSubscriptions.Value);
            }

            VkTaCommunitiesOptionsSm taCommunitiesOptions = null;
            VkCommunitiesSearchOptionsSm commSearchOptions = null;

            if (ResultType == VkParsingTaskResultType.Communities)
            {
                if (CommunitiesResultSubType == VkParsingTaskResultCommunitiesSubType.TaIntersection
                    && CommunitiesResultTaIntersectionCommunitiesCount.HasValue)
                {
                    taCommunitiesOptions = new VkTaCommunitiesOptionsSm(
                        CommunitiesResultTaIntersectionTopType,
                        CommunitiesResultTaIntersectionCommunitiesCount.Value);
                }
                
                if (CommunitiesResultSubType == VkParsingTaskResultCommunitiesSubType.CommunitiesSearch)
                {
                    commSearchOptions = new VkCommunitiesSearchOptionsSm(
                        CommunitiesResultCommSearchSearchType,
                        CommunitiesResultCommSearchGroupType,
                        CommunitiesResultCommSearchResultSort,
                        CommunitiesResultCommSearchMarketOnly,
                        CommunitiesResultCommSearchMembersMin,
                        CommunitiesResultCommSearchMembersMax,
                        CommunitiesResultCommSearchPhraseSearch,
                        CommunitiesResultCommSearchMinusWords,
                        CommunitiesResultCommSearchTrending,
                        CommunitiesResultCommSearchVerified);
                }
            }

            VkParsingTaskOptionsSm vkParsingTaskOptionsSm = new VkParsingTaskOptionsSm(
                OptionsSmRawLinkSources,
                OptionsSmRawTaskSources,
                OptionsSmSourceObjectsType,
                ProfilesResultSubType,
                CommunitiesResultSubType,
                topProfilesOptionsSm,
                activeProfilesOptionsSm,
                groupIntersectOptionsSm,
                friendsOptionsSm,
                taCommunitiesOptions,
                commSearchOptions,
                UpdateActiveUsersTimeFrame);

            VkCommunitiesFilterOptionsSm commFilterOptionsSm = null;
            
            if (LastCommWallPostPeriodStart.HasValue && LastCommWallPostPeriodStart != DateTime.MinValue
                && LastCommWallPostPeriodEnd.HasValue && LastCommWallPostPeriodEnd != DateTime.MinValue)
            {
                commFilterOptionsSm = new VkCommunitiesFilterOptionsSm(
                    LastCommWallPostPeriodStart.Value,
                    //VkCommunitiesFilterOptions.LastPostPeriodEnd
                    LastCommWallPostPeriodEnd.Value);
            }

            VkParsingTaskFilterOptionsSm filterOptionsSm = new VkParsingTaskFilterOptionsSm(
                FilterEnabled,
                commFilterOptionsSm);

            VkAdsAccountSm vkAdsAccountSm = VkAdsAccount != null ?
                new VkAdsAccountSm(VkAdsAccount.Id, VkAdsAccount.Name) : null;
            VkAdsTargetGroupSm vkAdsTargetGroupSm = VkAdsTargetGroup != null ?
                new VkAdsTargetGroupSm(VkAdsTargetGroup.Id, VkAdsTargetGroup.Name) : null;
            bool? createNewVkAdsTargetGroup = ExportToVkAds == false ? null :
                VkAdsTargetGroup != null ? null : CreateNewVkAdsTargetGroup;

            VkParsingTaskAutomationOptionsSm automationOptionsSm = new VkParsingTaskAutomationOptionsSm(
                CreatePeriodicTask,
                PeriodicTaskExecutionRate);

            VkParsingTaskVkAdsExportOptionsSm vkAdsExportOptionsSm = new VkParsingTaskVkAdsExportOptionsSm(
                ExportToVkAds,
                vkAdsAccountSm,
                vkAdsTargetGroupSm,
                createNewVkAdsTargetGroup,
                NewTargetGroupName);

            VkOneTimeParsingTaskSm vkParsingTaskSm = new VkOneTimeParsingTaskSm(
                OptionsSmTitleRawInput,
                SourceType,
                ResultType,
                true,
                vkParsingTaskOptionsSm,
                filterOptionsSm,
                automationOptionsSm,
                vkAdsExportOptionsSm);

            return vkParsingTaskSm;
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}
