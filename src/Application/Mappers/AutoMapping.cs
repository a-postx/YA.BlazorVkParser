namespace YA.WebClient.Application.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<UserVm, UserSm>().ReverseMap();
        CreateMap<UserSettingVm, UserSettingSm>().ReverseMap();
        CreateMap<TenantVm, TenantSm>().ReverseMap();
        CreateMap<VkOneTimeParsingTaskVm, VkOneTimeParsingTaskSm>().ReverseMap();
        CreateMap<VkPeriodicParsingTaskVm, VkPeriodicParsingTaskSm>().ReverseMap();
        CreateMap<VkParsingTaskOptionsVm, VkParsingTaskOptionsSm>().ReverseMap();
        CreateMap<VkActiveProfilesOptionsVm, VkActiveProfilesOptionsSm>().ReverseMap();
        CreateMap<VkActivitySourceOptionsVm, VkActivitySourceOptionsSm>().ReverseMap();
        CreateMap<VkActivityTypeOptionsVm, VkActivityTypeOptionsSm>().ReverseMap();
        CreateMap<VkTopProfilesOptionsVm, VkTopProfilesOptionsSm>().ReverseMap();
        CreateMap<VkGroupIntersectionProfilesOptionsVm, VkGroupIntersectionProfilesOptionsSm>().ReverseMap();
        CreateMap<VkFriendsProfilesOptionsVm, VkFriendsProfilesOptionsSm>().ReverseMap();
        CreateMap<VkTaCommunitiesOptionsVm, VkTaCommunitiesOptionsSm>().ReverseMap();
        CreateMap<VkCommunitiesSearchOptionsVm, VkCommunitiesSearchOptionsSm>().ReverseMap();
        CreateMap<VkParsingTaskAutomationOptionsVm, VkParsingTaskAutomationOptionsSm>().ReverseMap();
        CreateMap<VkAdsAccountVm, VkAdsAccountSm>().ReverseMap();
        CreateMap<VkAdsTargetGroupVm, VkAdsTargetGroupSm>().ReverseMap();
        CreateMap<VkParsingTaskFilterOptionsVm, VkParsingTaskFilterOptionsSm>().ReverseMap();
        CreateMap<VkCommunitiesFilterOptionsVm, VkCommunitiesFilterOptionsSm>().ReverseMap();
        CreateMap<VkOneTimeParsingTaskVm, VkQuickParsingTaskVm>();
    }
}
