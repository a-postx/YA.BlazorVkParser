using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YA.WebClient.Application.Enums;
using YA.WebClient.Application.Events;
using YA.WebClient.Application.Interfaces;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Application.Models.ViewModels;

namespace YA.WebClient.Application.Caches
{
    public class ParsingTaskModal : IDisposable
    {
        public ParsingTaskModal(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
            _modalInputTimer = new Timer(_ => NotifyPropertyChanged());
        }

        private readonly IApiRepository _apiRepository;

        private Timer _modalInputTimer;
        private bool _disposedValue;

        private bool _optionsEnabled;

        private string _optionsSmRawLinkSources;
        private string _optionsSmRawTaskSources;
        private string _optionsSmTitleRawInput = string.Empty;
        private bool _optionsSmUpdateActiveUsersTimeFrame = true;

        private string _whatToGetTooltipText = string.Empty;
        private string _whatUsersToGetTooltipText = string.Empty;
        private string _whatCommunitiesToGetTooltipText = string.Empty;

        private bool _autoNamingIsOn = true;

        private VkParsingTaskSourcesObjectType _optionsSmSourceObjectsType = VkParsingTaskSourcesObjectType.Unknown;

        private VkParsingTaskSourceType _optionsSmSourceType = VkParsingTaskSourceType.Unknown;
        private VkParsingTaskResultType _optionsSmResultType = VkParsingTaskResultType.Unknown;
        
        private VkParsingTaskResultProfilesSubType _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
        private VkParsingTaskResultCommunitiesSubType _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;

        private VkParsingTaskResultProfileTopType _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
        private int? _optionsTopSmTopCommunitiesCount;

        private VkParsingTaskResultCommunitiesTopType _optionsSmCommunityTaIntersectionTopType = VkParsingTaskResultCommunitiesTopType.Unknown;
        private int? _optionsSmCommunitiesTaIntersectionCommunitiesCount;

        private VkParsingTaskCommunitySearchSearchType _optionsSmCommunityCommSearchSearchType = VkParsingTaskCommunitySearchSearchType.Unknown;
        private VkParsingTaskCommunitySearchCommType _optionsSmCommunityCommSearchGroupType = VkParsingTaskCommunitySearchCommType.Unknown;
        private VkParsingTaskCommunitySearchResultSort _optionsSmCommunityCommSearchResultSort = VkParsingTaskCommunitySearchResultSort.Unknown;
        private bool _optionsSmCommunityCommSearchMarketOnly;
        private int? _optionsSmCommunityCommSearchMembersMin;
        private int? _optionsSmCommunityCommSearchMembersMax;
        private bool _optionsSmCommunityCommSearchPhraseSearch;
        private string _optionsSmCommunityCommSearchMinusWords;
        private bool _optionsSmCommunityCommSearchTrending;
        private bool _optionsSmCommunityCommSearchVerified;

        private bool? _optionsActiveSmSourcePosts;
        private bool? _optionsActiveSmSourceDiscussions;
        private bool? _optionsActiveSmTypeLikes;
        private bool? _optionsActiveSmTypeLikesInComments;
        private bool? _optionsActiveSmTypeComments;
        private bool? _optionsActiveSmTypeReposts;
        private bool? _optionsActiveSmTypePostAuthors;
        private DateTime _optionsActiveSmPeriodStart;
        private DateTime _optionsActiveSmPeriodEnd;
        private int? _optionsActiveSmActivityCountFrom;
        private bool? _optionsActiveSmLimitWallPostsCount;

        private int? _optionsGroupIntersectionSmCountFrom;
        private int? _optionsGroupIntersectionSmCountTo;

        private bool? _optionsFriendsSmGetFriends;
        private bool? _optionsFriendsSmGetFollowers;
        private bool? _optionsFriendsSmGetPeopleSubscriptions;

        private bool _filterOptionsSmEnabled;
        private DateTime _filterOptionsSmLastCommWallPostPeriodStart;
        private DateTime _filterOptionsSmLastCommWallPostPeriodEnd;

        private bool _automationOptionsSmCreatePeriodicTask;
        private bool _periodicTaskOptionsSmDisabled;
        private VkPeriodicParsingTaskRate? _automationOptionsSmPeriodicTaskExecutionRate;

        private bool _vkAdsExportOptionsSmExportToVkAds;
        private VkAdsAccountVm _vkAdsExportOptionsVmVkAdsAccount;
        private VkAdsTargetGroupVm _vkAdsExportOptionsVmVkAdsTargetGroup;
        private bool _vkAdsExportOptionsCreateNewVkAdsTargetGroup;
        private string _vkAdsExportOptionsNewTargetGroupName;

        private ICollection<VkAdsAccountVm> _vkAdsAccounts;
        private ICollection<VkAdsTargetGroupVm> _vkAdsTargetGroups;

        //селектор блейзорайс неправильно работает с непримитивными типами, поэтому дублируем
        private long _vkAdsAccountId;
        private long _vkAdsTargetGroupId;


        public ValidatedVkParsingTaskModalTm ValidatedForm { get; set; } = new ValidatedVkParsingTaskModalTm();
        
        public bool SourcesCountInvalid { get; set; }
        public bool ActivitySourceInvalid { get; set; }
        public bool ActivityTypeInvalid { get; set; }
        public bool FriendsTypeInvalid { get; set; }

        public bool SkipBlazoriseValidation { get; set; }

        /// <summary>
        /// Происходит при изменении любого свойства объекта, которое должно вызывать валидацию
        /// </summary>
        public event EventHandler PropertyChanged;
        /// <summary>
        /// Происходит при изменении свойства ExportToVkAds
        /// </summary>
        public event EventHandler<ExportToVkAdsChangedEventArgs> ExportToVkAdsChanged;

        private void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this, null);
        }

        private void NotifyExportToVkAdsChanged(bool value)
        {
            ExportToVkAdsChanged?.Invoke(this, new ExportToVkAdsChangedEventArgs { Value = value });
        }

        public bool OptionsEnabled
        {
            get
            {
                return _optionsEnabled;
            }

            set
            {
                if (value != _optionsEnabled)
                {
                    _optionsEnabled = value;

                    if (!_optionsEnabled)
                    {
                        HideAllChildOptions();
                    }
                    else
                    {
                        ShowAllChildOptions();
                    }
                }
            }
        }

        //либо аннотации либо проверка Блейзорайс
        [Required(ErrorMessage = "Укажите источники")]
        [StringLength(6000, MinimumLength = 1, ErrorMessage = "Слишком большое количество источников")]
        public string OptionsSmRawTaskSources
        {
            get
            {
                return _optionsSmRawTaskSources;
            }

            set
            {
                if (value != _optionsSmRawTaskSources)
                {
                    _optionsSmRawTaskSources = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        [Required(ErrorMessage = "Укажите источники")]
        [StringLength(6000, MinimumLength = 1, ErrorMessage = "Слишком большое количество источников")]
        public string OptionsSmRawLinkSources
        {
            get
            {
                return _optionsSmRawLinkSources;
            }

            set
            {
                if (value != _optionsSmRawLinkSources)
                {
                    _optionsSmRawLinkSources = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        [Required(ErrorMessage = "Название задачи не должно быть пустым")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Название должно быть от 1 до 1000 символов")]
        public string OptionsSmTitleRawInput
        {
            get
            {
                return _optionsSmTitleRawInput;
            }

            set
            {
                if (value != _optionsSmTitleRawInput)
                {
                    _optionsSmTitleRawInput = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public VkParsingTaskSourcesObjectType OptionsSmSourceObjectsType
        {
            get
            {
                return _optionsSmSourceObjectsType;
            }

            set
            {
                if (value != _optionsSmSourceObjectsType)
                {
                    _optionsSmSourceObjectsType = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool AutoNamingIsOn
        {
            get
            {
                return _autoNamingIsOn;
            }
            
            set
            {
                if (value != _autoNamingIsOn)
                {
                    _autoNamingIsOn = value;

                    if (_autoNamingIsOn)
                    {
                        ResetCheckingTimerAndDisableSubmitButton();
                    }
                }
            }
        }

        public bool SubmitButtonDisabled { get; set; } = true;

        public bool ProfileResultTypeDisabled { get; set; } = true;
        public bool CommunityResultTypeDisabled { get; set; } = true;

        public VkParsingTaskSourceType SourceType
        {
            get
            {
                return _optionsSmSourceType;
            }

            set
            {
                if (value != _optionsSmSourceType)
                {
                    _optionsSmSourceType = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool ResultTypeVisible { get; set; }
        public VkParsingTaskResultType ResultType
        {
            get
            {
                return _optionsSmResultType;
            }

            set
            {
                if (value != _optionsSmResultType)
                {
                    _optionsSmResultType = value;
                    UpdateOptionValuesOnResultTypeChange(value);
                    UpdateTooltipTexts();
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool ProfilesResultSubTypeVisible { get; set; }

        public VkParsingTaskResultProfilesSubType ProfilesResultSubType
        {
            get
            {
                return _optionsSmProfilesSubType;
            }

            set
            {
                if (value != _optionsSmProfilesSubType)
                {
                    _optionsSmProfilesSubType = value;
                    UpdateTooltipTexts();
                }
            }
        }

        public bool ProfilesResultAllTypeDisabled { get; set; }

        public bool ProfilesResultTopTypeVisible { get; set; }
        public bool ProfilesResultTopTypeDisabled { get; set; }
        public VkParsingTaskResultProfileTopType ProfilesResultTopType
        {
            get
            {
                return _optionsTopSmProfileTopType;
            }

            set
            {
                if (value != _optionsTopSmProfileTopType)
                {
                    _optionsTopSmProfileTopType = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        [Required(ErrorMessage = "Введите целое положительное число")]
        [Range(1, int.MaxValue, ErrorMessage = "Введите целое положительное число")]
        public int? TopCommunitiesCount
        {
            get
            {
                return _optionsTopSmTopCommunitiesCount;
            }

            set
            {
                if (value != _optionsTopSmTopCommunitiesCount)
                {
                    _optionsTopSmTopCommunitiesCount = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool ProfilesResultActiveTypeDisabled { get; set; }
        public bool ProfilesResultActiveTypeVisible { get; set; }
        public bool? ProfilesResultActiveSourcePosts
        {
            get
            {
                return _optionsActiveSmSourcePosts;
            }

            set
            {
                if (value != _optionsActiveSmSourcePosts)
                {
                    _optionsActiveSmSourcePosts = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveSourceDiscussions
        {
            get
            {
                return _optionsActiveSmSourceDiscussions;
            }

            set
            {
                if (value != _optionsActiveSmSourceDiscussions)
                {
                    _optionsActiveSmSourceDiscussions = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveTypeLikes
        {
            get
            {
                return _optionsActiveSmTypeLikes;
            }

            set
            {
                if (value != _optionsActiveSmTypeLikes)
                {
                    _optionsActiveSmTypeLikes = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveTypeLikesInComments
        {
            get
            {
                return _optionsActiveSmTypeLikesInComments;
            }

            set
            {
                if (value != _optionsActiveSmTypeLikesInComments)
                {
                    _optionsActiveSmTypeLikesInComments = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveTypeComments
        {
            get
            {
                return _optionsActiveSmTypeComments;
            }

            set
            {
                if (value != _optionsActiveSmTypeComments)
                {
                    _optionsActiveSmTypeComments = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveTypeReposts
        {
            get
            {
                return _optionsActiveSmTypeReposts;
            }

            set
            {
                if (value != _optionsActiveSmTypeReposts)
                {
                    _optionsActiveSmTypeReposts = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveTypePostAuthors
        {
            get
            {
                return _optionsActiveSmTypePostAuthors;
            }

            set
            {
                if (value != _optionsActiveSmTypePostAuthors)
                {
                    _optionsActiveSmTypePostAuthors = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public DateTime? ProfilesResultActivePeriodStart
        {
            get
            {
                return _optionsActiveSmPeriodStart;
            }

            set
            {
                if (value != _optionsActiveSmPeriodStart)
                {
                    if (value.HasValue)
                    {
                        _optionsActiveSmPeriodStart = SetLocalDateTimeForBrowserCalendarInput(value.Value);
                        UpdateOptionsVisibilityAndResetTimer();
                    }
                    else
                    {
                        throw new InvalidOperationException("no value");  // никогда не вызывается
                    }
                }
            }
        }

        public DateTime? ProfilesResultActivePeriodEnd
        {
            get
            {
                return _optionsActiveSmPeriodEnd;
            }

            set
            {
                if (value != _optionsActiveSmPeriodEnd)
                {
                    if (value.HasValue)
                    {
                        _optionsActiveSmPeriodEnd = SetLocalDateTimeForBrowserCalendarInput(value.Value);
                        UpdateOptionsVisibilityAndResetTimer();
                    }
                    else
                    {
                        throw new InvalidOperationException("no value");  // никогда не вызывается
                    }
                }
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Введите целое положительное число")]
        public int? ProfilesResultActiveActivityCountFrom
        {
            get
            {
                return _optionsActiveSmActivityCountFrom;
            }

            set
            {
                if (value != _optionsActiveSmActivityCountFrom)
                {
                    _optionsActiveSmActivityCountFrom = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool? ProfilesResultActiveLimitWallPostsCount
        {
            get
            {
                return _optionsActiveSmLimitWallPostsCount;
            }

            set
            {
                if (value != _optionsActiveSmLimitWallPostsCount)
                {
                    _optionsActiveSmLimitWallPostsCount = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool ProfilesResultGroupIntersectionTypeDisabled { get; set; }
        public bool ProfilesResultGroupIntersectionTypeVisible { get; set; }

        [Required(ErrorMessage = "Введите целое положительное число")]
        [Range(1, int.MaxValue, ErrorMessage = "Введите целое положительное число")]
        public int? ProfilesResultGiCountFrom
        {
            get
            {
                return _optionsGroupIntersectionSmCountFrom;
            }

            set
            {
                if (value != _optionsGroupIntersectionSmCountFrom)
                {
                    _optionsGroupIntersectionSmCountFrom = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        [Required(ErrorMessage = "Введите целое положительное число")]
        [Range(1, int.MaxValue, ErrorMessage = "Введите целое положительное число")]
        public int? ProfilesResultGiCountTo
        {
            get
            {
                return _optionsGroupIntersectionSmCountTo;
            }

            set
            {
                if (value != _optionsGroupIntersectionSmCountTo)
                {
                    _optionsGroupIntersectionSmCountTo = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool ProfilesResultFriendsTypeDisabled { get; set; }
        public bool ProfilesResultFriendsTypeVisible { get; set; }

        public bool? ProfilesResultFriendsGetFriends
        {
            get
            {
                return _optionsFriendsSmGetFriends;
            }

            set
            {
                if (value != _optionsFriendsSmGetFriends)
                {
                    _optionsFriendsSmGetFriends = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool? ProfilesResultFriendsGetFollowers
        {
            get
            {
                return _optionsFriendsSmGetFollowers;
            }

            set
            {
                if (value != _optionsFriendsSmGetFollowers)
                {
                    _optionsFriendsSmGetFollowers = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool? ProfilesResultFriendsGetPeopleSubscriptions
        {
            get
            {
                return _optionsFriendsSmGetPeopleSubscriptions;
            }

            set
            {
                if (value != _optionsFriendsSmGetPeopleSubscriptions)
                {
                    _optionsFriendsSmGetPeopleSubscriptions = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool ProfilesResultDetailedProfilesTypeDisabled { get; set; }


        public bool CommunitiesResultSubTypeDisabled { get; set; }
        public bool CommunitiesResultSubTypeVisible { get; set; }
        public VkParsingTaskResultCommunitiesSubType CommunitiesResultSubType
        {
            get
            {
                return _optionsSmCommunitiesSubType;
            }

            set
            {
                if (value != _optionsSmCommunitiesSubType)
                {
                    _optionsSmCommunitiesSubType = value;
                    UpdateOptionValuesOnCommunitiesResultSubTypeChange(value);
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool CommunitiesResultTaIntersectionTypeDisabled { get; set; } = true;
        public bool CommunitiesResultTaIntersectionTypeVisible { get; set; }
        public VkParsingTaskResultCommunitiesTopType CommunitiesResultTaIntersectionTopType
        {
            get
            {
                return _optionsSmCommunityTaIntersectionTopType;
            }

            set
            {
                if (value != _optionsSmCommunityTaIntersectionTopType)
                {
                    _optionsSmCommunityTaIntersectionTopType = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        [Required(ErrorMessage = "Введите целое положительное число")]
        [Range(1, 10000, ErrorMessage = "Введите целое положительное число от 1 до 10000")]
        public int? CommunitiesResultTaIntersectionCommunitiesCount
        {
            get
            {
                return _optionsSmCommunitiesTaIntersectionCommunitiesCount;
            }

            set
            {
                if (value != _optionsSmCommunitiesTaIntersectionCommunitiesCount)
                {
                    _optionsSmCommunitiesTaIntersectionCommunitiesCount = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool CommunitiesResultCommSearchTypeDisabled { get; set; } = true;
        public bool CommunitiesResultCommSearchTypeVisible { get; set; }

        public VkParsingTaskCommunitySearchSearchType CommunitiesResultCommSearchSearchType
        {
            get
            {
                return _optionsSmCommunityCommSearchSearchType;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchSearchType)
                {
                    _optionsSmCommunityCommSearchSearchType = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public VkParsingTaskCommunitySearchCommType CommunitiesResultCommSearchGroupType
        {
            get
            {
                return _optionsSmCommunityCommSearchGroupType;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchGroupType)
                {
                    _optionsSmCommunityCommSearchGroupType = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public VkParsingTaskCommunitySearchResultSort CommunitiesResultCommSearchResultSort
        {
            get
            {
                return _optionsSmCommunityCommSearchResultSort;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchResultSort)
                {
                    _optionsSmCommunityCommSearchResultSort = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool CommunitiesResultCommSearchMarketOnly
        {
            get
            {
                return _optionsSmCommunityCommSearchMarketOnly;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchMarketOnly)
                {
                    _optionsSmCommunityCommSearchMarketOnly = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public int? CommunitiesResultCommSearchMembersMin
        {
            get
            {
                return _optionsSmCommunityCommSearchMembersMin;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchMembersMin)
                {
                    _optionsSmCommunityCommSearchMembersMin = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public int? CommunitiesResultCommSearchMembersMax
        {
            get
            {
                return _optionsSmCommunityCommSearchMembersMax;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchMembersMax)
                {
                    _optionsSmCommunityCommSearchMembersMax = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool CommunitiesResultCommSearchPhraseSearch
        {
            get
            {
                return _optionsSmCommunityCommSearchPhraseSearch;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchPhraseSearch)
                {
                    _optionsSmCommunityCommSearchPhraseSearch = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public string CommunitiesResultCommSearchMinusWords
        {
            get
            {
                return _optionsSmCommunityCommSearchMinusWords;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchMinusWords)
                {
                    _optionsSmCommunityCommSearchMinusWords = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool CommunitiesResultCommSearchTrending
        {
            get
            {
                return _optionsSmCommunityCommSearchTrending;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchTrending)
                {
                    _optionsSmCommunityCommSearchTrending = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }

        public bool CommunitiesResultCommSearchVerified
        {
            get
            {
                return _optionsSmCommunityCommSearchVerified;
            }

            set
            {
                if (value != _optionsSmCommunityCommSearchVerified)
                {
                    _optionsSmCommunityCommSearchVerified = value;
                    ResetCheckingTimerAndDisableSubmitButton();
                }
            }
        }


        public bool FilterSectionVisible { get; set; }
        public bool FilterEnabled
        {
            get
            {
                return _filterOptionsSmEnabled;
            }

            set
            {
                if (value != _filterOptionsSmEnabled)
                {
                    _filterOptionsSmEnabled = value;

                    UpdateOptionValuesOnFilterEnabledChange(value);
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool CommunityFilterOptionsVisible { get; set; }

        public DateTime? LastCommWallPostPeriodStart
        {
            get
            {
                return _filterOptionsSmLastCommWallPostPeriodStart;
            }

            set
            {
                if (value != _filterOptionsSmLastCommWallPostPeriodStart)
                {
                    if (value.HasValue)
                    {
                        _filterOptionsSmLastCommWallPostPeriodStart = SetLocalDateTimeForBrowserCalendarInput(value.Value);
                        UpdateOptionsVisibilityAndResetTimer();
                    }
                    else
                    {
                        throw new InvalidOperationException("no value");  // никогда не вызывается
                    }
                }
            }
        }

        public DateTime? LastCommWallPostPeriodEnd
        {
            get
            {
                return _filterOptionsSmLastCommWallPostPeriodEnd;
            }

            set
            {
                if (value != _filterOptionsSmLastCommWallPostPeriodEnd)
                {
                    if (value.HasValue)
                    {
                        _filterOptionsSmLastCommWallPostPeriodEnd = SetLocalDateTimeForBrowserCalendarInput(value.Value);
                        UpdateOptionsVisibilityAndResetTimer();
                    }
                    else
                    {
                        throw new InvalidOperationException("no value");  // никогда не вызывается
                    }
                }
            }
        }


        public bool AutomationSectionVisible { get; set; }

        public bool CreatePeriodicTask
        {
            get
            {
                return _automationOptionsSmCreatePeriodicTask;
            }

            set
            {
                if (value != _automationOptionsSmCreatePeriodicTask)
                {
                    _automationOptionsSmCreatePeriodicTask = value;

                    UpdateOptionValuesOnCreatePeriodicTaskChange(value);
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool PeriodicTaskOptionsVisible { get; set; }

        public bool PeriodicTaskOptionsDisabled
        {
            get
            {
                return _periodicTaskOptionsSmDisabled;
            }

            set
            {
                if (value != _periodicTaskOptionsSmDisabled)
                {
                    _periodicTaskOptionsSmDisabled = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public VkPeriodicParsingTaskRate? PeriodicTaskExecutionRate
        {
            get
            {
                return _automationOptionsSmPeriodicTaskExecutionRate;
            }

            set
            {
                if (value != _automationOptionsSmPeriodicTaskExecutionRate)
                {
                    _automationOptionsSmPeriodicTaskExecutionRate = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool VkAdsExportSectionVisible { get; set; }
        public bool ExportToVkAds
        {
            get
            {
                return _vkAdsExportOptionsSmExportToVkAds;
            }

            set
            {
                if (value != _vkAdsExportOptionsSmExportToVkAds)
                {
                    _vkAdsExportOptionsSmExportToVkAds = value;

                    UpdateOptionValuesOnExportToVkAdsChange(value);
                    NotifyExportToVkAdsChanged(value);
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool VkAdsAccountSelectorVisible { get; set; }
        public VkAdsAccountVm VkAdsAccount
        {
            get
            {
                return _vkAdsExportOptionsVmVkAdsAccount;
            }

            set
            {
                if (value != _vkAdsExportOptionsVmVkAdsAccount)
                {
                    _vkAdsExportOptionsVmVkAdsAccount = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool VkAdsTargetGroupsSelectorVisible { get; set; }
        public VkAdsTargetGroupVm VkAdsTargetGroup
        {
            get
            {
                return _vkAdsExportOptionsVmVkAdsTargetGroup;
            }

            set
            {
                if (value != _vkAdsExportOptionsVmVkAdsTargetGroup)
                {
                    _vkAdsExportOptionsVmVkAdsTargetGroup = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool CreateNewVkAdsTargetGroup
        {
            get
            {
                return _vkAdsExportOptionsCreateNewVkAdsTargetGroup;
            }

            set
            {
                if (value != _vkAdsExportOptionsCreateNewVkAdsTargetGroup)
                {
                    _vkAdsExportOptionsCreateNewVkAdsTargetGroup = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public bool NewTargetGroupNameSelectorVisible { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 64 символов")]
        public string NewTargetGroupName
        {
            get
            {
                return _vkAdsExportOptionsNewTargetGroupName;
            }

            set
            {
                if (value != _vkAdsExportOptionsNewTargetGroupName)
                {
                    _vkAdsExportOptionsNewTargetGroupName = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public ICollection<VkAdsAccountVm> AvailableVkAdsAccounts
        {
            get
            {
                return _vkAdsAccounts;
            }

            set
            {
                if (value != _vkAdsAccounts)
                {
                    _vkAdsAccounts = value;
                }
            }
        }

        public ICollection<VkAdsTargetGroupVm> AvailableVkAdsTargetGroups
        {
            get
            {
                return _vkAdsTargetGroups;
            }

            set
            {
                if (value != _vkAdsTargetGroups)
                {
                    _vkAdsTargetGroups = value;
                }
            }
        }

        public long VkAdsAccountId
        {
            get
            {
                return _vkAdsAccountId;
            }

            set
            {
                if (value != _vkAdsAccountId)
                {
                    _vkAdsAccountId = value;
                }
            }
        }

        public long VkAdsTargetGroupId
        {
            get
            {
                return _vkAdsTargetGroupId;
            }

            set
            {
                if (value != _vkAdsTargetGroupId)
                {
                    _vkAdsTargetGroupId = value;
                }
            }
        }

        public bool UpdateActiveUsersTimeFrame
        {
            get
            {
                return _optionsSmUpdateActiveUsersTimeFrame;
            }

            set
            {
                if (value != _optionsSmUpdateActiveUsersTimeFrame)
                {
                    _optionsSmUpdateActiveUsersTimeFrame = value;
                    UpdateOptionsVisibilityAndResetTimer();
                }
            }
        }

        public string WhatToGetTooltipText
        {
            get
            {
                return _whatToGetTooltipText;
            }

            set
            {
                if (value != _whatToGetTooltipText)
                {
                    _whatToGetTooltipText = value;
                }
            }
        }

        public string WhatUsersToGetTooltipText
        {
            get
            {
                return _whatUsersToGetTooltipText;
            }

            set
            {
                if (value != _whatUsersToGetTooltipText)
                {
                    _whatUsersToGetTooltipText = value;
                }
            }
        }

        public string WhatCommunitiesToGetTooltipText
        {
            get
            {
                return _whatCommunitiesToGetTooltipText;
            }

            set
            {
                if (value != _whatCommunitiesToGetTooltipText)
                {
                    _whatCommunitiesToGetTooltipText = value;
                }
            }
        }

        


        private void ResetCheckingTimerAndDisableSubmitButton()
        {
            SubmitButtonDisabled = true;

            try
            {
                _modalInputTimer.Change(1000, Timeout.Infinite);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        public void ResetCheckingTimer(int initialDelayMsec)
        {
            try
            {
                _modalInputTimer.Change(initialDelayMsec, Timeout.Infinite);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        public void UpdateOptionsVisibilityAndResetTimer()
        {
            UpdateOptionsVisibility();
            ResetCheckingTimerAndDisableSubmitButton();
        }

        public void UpdateOptionsVisibility()
        {
            UpdateMainOptionsVisibility();
            UpdateFilterOptionsVisibility();
            UpdateAutomationOptionsVisibility();
            UpdateVkAdsExportOptionsVisibility();
        }

        private void UpdateMainOptionsVisibility()
        {
            switch (ResultType)
            {
                case VkParsingTaskResultType.Profiles:
                    ProfilesResultSubTypeVisible = true;

                    CommunitiesResultSubTypeVisible = false;
                    CommunitiesResultTaIntersectionTypeVisible = false;
                    CommunitiesResultCommSearchTypeVisible = false;

                    break;
                case VkParsingTaskResultType.Communities:
                    CommunitiesResultSubTypeVisible = true;

                    ProfilesResultSubTypeVisible = false;
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;

                    break;
                case VkParsingTaskResultType.VkAdsExport:
                    VkAdsExportSectionVisible = true;
                    VkAdsAccountSelectorVisible = true;
                    VkAdsTargetGroupsSelectorVisible = true;

                    CommunitiesResultSubTypeVisible = false;
                    ProfilesResultSubTypeVisible = false;
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;

                    break;
                default:
                case VkParsingTaskResultType.Unknown:
                    ProfilesResultSubTypeVisible = false;

                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;

                    CommunitiesResultSubTypeVisible = false;

                    CommunitiesResultTaIntersectionTypeVisible = false;
                    CommunitiesResultCommSearchTypeVisible = false;

                    break;
            }

            switch (ProfilesResultSubType)
            {
                case VkParsingTaskResultProfilesSubType.All:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
                case VkParsingTaskResultProfilesSubType.Top:
                    ProfilesResultTopTypeVisible = true;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
                case VkParsingTaskResultProfilesSubType.Active:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = true;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
                case VkParsingTaskResultProfilesSubType.GroupIntersection:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = true;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
                case VkParsingTaskResultProfilesSubType.Friends:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = true;
                    break;
                case VkParsingTaskResultProfilesSubType.DetailedProfiles:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
                default:
                case VkParsingTaskResultProfilesSubType.Unknown:
                    ProfilesResultTopTypeVisible = false;
                    ProfilesResultActiveTypeVisible = false;
                    ProfilesResultGroupIntersectionTypeVisible = false;
                    ProfilesResultFriendsTypeVisible = false;
                    break;
            }

            switch (CommunitiesResultSubType)
            {
                case VkParsingTaskResultCommunitiesSubType.TaIntersection:
                    CommunitiesResultTaIntersectionTypeVisible = true;
                    CommunitiesResultCommSearchTypeVisible = false;
                    break;
                case VkParsingTaskResultCommunitiesSubType.CommunitiesSearch:
                    CommunitiesResultCommSearchTypeVisible = true;
                    CommunitiesResultTaIntersectionTypeVisible = false;
                    break;
                default:
                case VkParsingTaskResultCommunitiesSubType.Unknown:
                    CommunitiesResultCommSearchTypeVisible = false;
                    CommunitiesResultTaIntersectionTypeVisible = false;
                    break;
            }
        }

        private void UpdateFilterOptionsVisibility()
        {
            FilterSectionVisible = ResultType == VkParsingTaskResultType.Communities
                && CommunitiesResultSubType != VkParsingTaskResultCommunitiesSubType.Unknown;

            CommunityFilterOptionsVisible = FilterEnabled;
        }

        private void UpdateAutomationOptionsVisibility()
        {
            AutomationSectionVisible = ResultType != VkParsingTaskResultType.Unknown 
                && (ProfilesResultSubType != VkParsingTaskResultProfilesSubType.Unknown 
                || CommunitiesResultSubType != VkParsingTaskResultCommunitiesSubType.Unknown);
        }

        private void UpdateVkAdsExportOptionsVisibility()
        {
            VkAdsExportSectionVisible = ResultType != VkParsingTaskResultType.Unknown
                && (ProfilesResultSubType != VkParsingTaskResultProfilesSubType.Unknown
                || CommunitiesResultSubType != VkParsingTaskResultCommunitiesSubType.Unknown);

            PeriodicTaskOptionsVisible = CreatePeriodicTask;
            VkAdsAccountSelectorVisible = ExportToVkAds;
            VkAdsTargetGroupsSelectorVisible = VkAdsAccount != null;
            NewTargetGroupNameSelectorVisible = CreateNewVkAdsTargetGroup;
        }

        public void SetTaskOptionsAvailabilityBasedOnLoadedModelForEdit()
        {
            switch (_optionsSmSourceObjectsType)
            {
                case VkParsingTaskSourcesObjectType.Group:
                    ProfilesResultAllTypeDisabled = false;
                    ProfilesResultActiveTypeDisabled = false;
                    ProfilesResultTopTypeDisabled = false;
                    ProfilesResultGroupIntersectionTypeDisabled = false;
                    ProfilesResultFriendsTypeDisabled = true;
                    ProfilesResultDetailedProfilesTypeDisabled = true;
                    CommunitiesResultTaIntersectionTypeDisabled = true;
                    CommunitiesResultCommSearchTypeDisabled = true;
                    break;
                case VkParsingTaskSourcesObjectType.User:
                    ProfilesResultAllTypeDisabled = true;
                    ProfilesResultActiveTypeDisabled = false;
                    ProfilesResultTopTypeDisabled = true;
                    ProfilesResultGroupIntersectionTypeDisabled = false;
                    ProfilesResultFriendsTypeDisabled = false;
                    ProfilesResultDetailedProfilesTypeDisabled = false;
                    CommunitiesResultTaIntersectionTypeDisabled = true;
                    CommunitiesResultCommSearchTypeDisabled = true;
                    break;
                case VkParsingTaskSourcesObjectType.Keyword:
                    ProfilesResultAllTypeDisabled = true;
                    ProfilesResultActiveTypeDisabled = true;
                    ProfilesResultTopTypeDisabled = true;
                    ProfilesResultGroupIntersectionTypeDisabled = true;
                    ProfilesResultFriendsTypeDisabled = true;
                    ProfilesResultDetailedProfilesTypeDisabled = true;
                    CommunitiesResultTaIntersectionTypeDisabled = true;
                    CommunitiesResultCommSearchTypeDisabled = false;
                    break;
                default:
                case VkParsingTaskSourcesObjectType.Unknown:
                    ProfilesResultAllTypeDisabled = true;
                    ProfilesResultActiveTypeDisabled = true;
                    ProfilesResultTopTypeDisabled = true;
                    ProfilesResultGroupIntersectionTypeDisabled = true;
                    ProfilesResultFriendsTypeDisabled = true;
                    ProfilesResultDetailedProfilesTypeDisabled = true;
                    CommunitiesResultTaIntersectionTypeDisabled = true;
                    CommunitiesResultCommSearchTypeDisabled = true;
                    break;
            }
        }

        public void UpdateTaskOptionsFromValidatedFormAndSetAvailability()
        {
            UpdateTaskOptionsAvailabilityFlags();

            //при смене видимости опций валидация блейзорайс плохо срабатывает, поэтому отключаем
            if (_optionsSmSourceObjectsType != ValidatedForm.SourcesObjectType)
            {
                _optionsSmSourceObjectsType = ValidatedForm.SourcesObjectType;

                SkipBlazoriseValidation = true;
            }

            VkParsingTaskResultType currentResultType = ResultType;

            //опция текущего типа результата стала недоступна
            if ((currentResultType == VkParsingTaskResultType.Profiles && ProfileResultTypeDisabled)
                || (currentResultType == VkParsingTaskResultType.Communities && CommunityResultTypeDisabled))
            {
                //проставляем внутренние переменные чтобы избежать повторного запроса проверки
                if (currentResultType == VkParsingTaskResultType.Profiles && ProfileResultTypeDisabled)
                {
                    if (!CommunityResultTypeDisabled)
                    {
                        _optionsSmResultType = VkParsingTaskResultType.Communities;
                        UpdateOptionValuesOnResultTypeChange(_optionsSmResultType);
                        _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
                        UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
                        _optionsSmCommunitiesSubType = GetNextAvailableCommunityResultSubType(CommunitiesResultSubType);
                        UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
                    }
                    else
                    {
                        _optionsSmResultType = VkParsingTaskResultType.Unknown;
                        _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
                        UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
                        _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;
                        UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
                    }
                }

                if (currentResultType == VkParsingTaskResultType.Communities && CommunityResultTypeDisabled)
                {
                    if (!ProfileResultTypeDisabled)
                    {
                        _optionsSmResultType = VkParsingTaskResultType.Profiles;
                        UpdateOptionValuesOnResultTypeChange(_optionsSmResultType);
                        _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;
                        UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
                        _optionsSmProfilesSubType = GetNextAvailableProfileResultSubType(ProfilesResultSubType);
                        UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
                    }
                    else
                    {
                        _optionsSmResultType = VkParsingTaskResultType.Unknown;
                        _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;
                        UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
                        _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
                        UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
                    }
                }
            }
            else
            {
                VkParsingTaskResultProfilesSubType currentProfileSubType = ProfilesResultSubType;

                //опция текущего подрезультата стала недоступна
                if (((currentProfileSubType == VkParsingTaskResultProfilesSubType.All && ProfilesResultAllTypeDisabled)
                    || (currentProfileSubType == VkParsingTaskResultProfilesSubType.Active && ProfilesResultActiveTypeDisabled)
                    || (currentProfileSubType == VkParsingTaskResultProfilesSubType.Top && ProfilesResultTopTypeDisabled)
                    || (currentProfileSubType == VkParsingTaskResultProfilesSubType.GroupIntersection && ProfilesResultGroupIntersectionTypeDisabled)
                    || (currentProfileSubType == VkParsingTaskResultProfilesSubType.Friends && ProfilesResultFriendsTypeDisabled)
                    || (currentProfileSubType == VkParsingTaskResultProfilesSubType.DetailedProfiles && ProfilesResultDetailedProfilesTypeDisabled))
                    && _optionsSmResultType == VkParsingTaskResultType.Profiles)
                {
                    _optionsSmProfilesSubType = GetNextAvailableProfileResultSubType(currentProfileSubType);
                    UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
                }

                VkParsingTaskResultCommunitiesSubType currentCommunitiesSubType = CommunitiesResultSubType;

                if (((currentCommunitiesSubType == VkParsingTaskResultCommunitiesSubType.TaIntersection && CommunitiesResultTaIntersectionTypeDisabled)
                    || (currentCommunitiesSubType == VkParsingTaskResultCommunitiesSubType.CommunitiesSearch && CommunitiesResultCommSearchTypeDisabled))
                    && _optionsSmResultType == VkParsingTaskResultType.Communities)
                {
                    _optionsSmCommunitiesSubType = GetNextAvailableCommunityResultSubType(currentCommunitiesSubType);
                    UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
                }
            }

            UpdateTooltipTexts();
        }

        private void UpdateTaskOptionsAvailabilityFlags()
        {
            ProfileResultTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.All)
                || ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Active)
                || ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Top)
                || ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.GroupIntersection)
                || ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Friends) 
                || ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.DetailedProfiles) ? false : true;
            CommunityResultTypeDisabled = ValidatedForm.EnabledCommunityParsers
                .HasFlag(EnabledCommunityParsers.TaIntersectionSearch)
                || ValidatedForm.EnabledCommunityParsers
                .HasFlag(EnabledCommunityParsers.CommunitySearch) ? false : true;

            ProfilesResultAllTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.All) ? false : true;
            ProfilesResultActiveTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Active) ? false : true;
            ProfilesResultTopTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Top) ? false : true;
            ProfilesResultGroupIntersectionTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.GroupIntersection) ? false : true;
            ProfilesResultFriendsTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.Friends) ? false : true;
            ProfilesResultDetailedProfilesTypeDisabled = ValidatedForm.EnabledProfileParsers
                .HasFlag(EnabledProfileParsers.DetailedProfiles) ? false : true;

            CommunitiesResultTaIntersectionTypeDisabled = ValidatedForm.EnabledCommunityParsers
                .HasFlag(EnabledCommunityParsers.TaIntersectionSearch) ? false : true;
            CommunitiesResultCommSearchTypeDisabled = ValidatedForm.EnabledCommunityParsers
                .HasFlag(EnabledCommunityParsers.CommunitySearch) ? false : true;
        }

        private VkParsingTaskResultProfilesSubType GetNextAvailableProfileResultSubType(VkParsingTaskResultProfilesSubType currentSubType)
        {
            switch (currentSubType)
            {
                case VkParsingTaskResultProfilesSubType.Unknown:
                    return ProfilesResultActiveTypeDisabled
                            ? ProfilesResultTopTypeDisabled
                                ? ProfilesResultGroupIntersectionTypeDisabled
                                    ? ProfilesResultFriendsTypeDisabled
                                        ? ProfilesResultAllTypeDisabled
                                            ? throw new InvalidOperationException("No profile subtype available.")
                                            : VkParsingTaskResultProfilesSubType.All
                                        : VkParsingTaskResultProfilesSubType.Friends
                                    : VkParsingTaskResultProfilesSubType.GroupIntersection
                                : VkParsingTaskResultProfilesSubType.Top
                            : VkParsingTaskResultProfilesSubType.Active;
                case VkParsingTaskResultProfilesSubType.All:
                    return ProfilesResultActiveTypeDisabled
                            ? ProfilesResultTopTypeDisabled
                                ? ProfilesResultGroupIntersectionTypeDisabled
                                    ? ProfilesResultFriendsTypeDisabled
                                        ? throw new InvalidOperationException("No profile subtype available.")
                                        : VkParsingTaskResultProfilesSubType.Friends
                                    : VkParsingTaskResultProfilesSubType.GroupIntersection
                                : VkParsingTaskResultProfilesSubType.Top
                            : VkParsingTaskResultProfilesSubType.Active;
                case VkParsingTaskResultProfilesSubType.Active:
                    return ProfilesResultTopTypeDisabled
                            ? ProfilesResultGroupIntersectionTypeDisabled
                                ? ProfilesResultFriendsTypeDisabled
                                    ? ProfilesResultAllTypeDisabled
                                        ? throw new InvalidOperationException("No profile subtype available.")
                                        : VkParsingTaskResultProfilesSubType.All
                                    : VkParsingTaskResultProfilesSubType.Friends
                                : VkParsingTaskResultProfilesSubType.GroupIntersection
                            : VkParsingTaskResultProfilesSubType.Top;
                case VkParsingTaskResultProfilesSubType.Top:
                    return ProfilesResultGroupIntersectionTypeDisabled
                            ? ProfilesResultFriendsTypeDisabled
                                ? ProfilesResultAllTypeDisabled
                                    ? ProfilesResultActiveTypeDisabled
                                        ? throw new InvalidOperationException("No profile subtype available.")
                                        : VkParsingTaskResultProfilesSubType.Active
                                    : VkParsingTaskResultProfilesSubType.All
                                : VkParsingTaskResultProfilesSubType.Friends
                            : VkParsingTaskResultProfilesSubType.GroupIntersection;
                case VkParsingTaskResultProfilesSubType.GroupIntersection:
                    return ProfilesResultFriendsTypeDisabled
                            ? ProfilesResultAllTypeDisabled
                                ? ProfilesResultActiveTypeDisabled
                                    ? ProfilesResultTopTypeDisabled
                                        ? throw new InvalidOperationException("No profile subtype available.")
                                        : VkParsingTaskResultProfilesSubType.Top
                                    : VkParsingTaskResultProfilesSubType.Active
                                : VkParsingTaskResultProfilesSubType.All
                            : VkParsingTaskResultProfilesSubType.Friends;
                case VkParsingTaskResultProfilesSubType.Friends:
                    return ProfilesResultAllTypeDisabled
                            ? ProfilesResultActiveTypeDisabled
                                ? ProfilesResultTopTypeDisabled
                                    ? ProfilesResultGroupIntersectionTypeDisabled
                                        ? throw new InvalidOperationException("No profile subtype available.")
                                        : VkParsingTaskResultProfilesSubType.GroupIntersection
                                    : VkParsingTaskResultProfilesSubType.Top
                                : VkParsingTaskResultProfilesSubType.Active
                            : VkParsingTaskResultProfilesSubType.All;
                default:
                    throw new InvalidOperationException("Unknown parsing task profile sub-type.");
            }
        }

        private VkParsingTaskResultCommunitiesSubType GetNextAvailableCommunityResultSubType(VkParsingTaskResultCommunitiesSubType currentSubType)
        {
            switch (currentSubType)
            {
                case VkParsingTaskResultCommunitiesSubType.Unknown:
                    return CommunitiesResultTaIntersectionTypeDisabled
                            ? CommunitiesResultCommSearchTypeDisabled
                                ? throw new InvalidOperationException("No community subtype available.")
                                : VkParsingTaskResultCommunitiesSubType.CommunitiesSearch
                            : VkParsingTaskResultCommunitiesSubType.TaIntersection;
                case VkParsingTaskResultCommunitiesSubType.TaIntersection:
                    return CommunitiesResultCommSearchTypeDisabled
                            ? throw new InvalidOperationException("No community subtype available.")
                            : VkParsingTaskResultCommunitiesSubType.CommunitiesSearch;
                case VkParsingTaskResultCommunitiesSubType.CommunitiesSearch:
                    return CommunitiesResultTaIntersectionTypeDisabled
                            ? throw new InvalidOperationException("No community subtype available.")
                            : VkParsingTaskResultCommunitiesSubType.TaIntersection;
                default:
                    throw new InvalidOperationException("Unknown parsing task community sub-type.");
            }
        }

        private int GetInputSourcesRowsCount(string input)
        {
            int result = 0;

            if (!string.IsNullOrWhiteSpace(input))
            {
                List<string> vkSourceIdCandidates = input.Split(Environment.NewLine).ToList();

                result = vkSourceIdCandidates.Count;
            }

            return result;
        }

        public bool ValidateSources()
        {
            int parsedSourcesCount = (SourceType == VkParsingTaskSourceType.Links)
                ? GetInputSourcesRowsCount(OptionsSmRawLinkSources)
                : GetInputSourcesRowsCount(OptionsSmRawTaskSources);

            SourcesCountInvalid = parsedSourcesCount > ((SourceType == VkParsingTaskSourceType.Links) ? 1000 : 10000000);

            if (parsedSourcesCount > 0 && !SourcesCountInvalid)
            {
                return true;
            }
            else
            {
                OnNoSourcesFound();
                return false;
            }
        }

        public async Task<bool> ValidateRemotelyAsync()
        {
            VkParsingTaskModalTm parsingTaskModalToValidate = new VkParsingTaskModalTm
            {
                SourceType = SourceType,
                RawLinkSources = OptionsSmRawLinkSources,
                RawTaskSources = OptionsSmRawTaskSources,
                ResultType = ResultType,
                ProfilesResultSubType = ProfilesResultSubType,
                ProfilesResultTopType = ProfilesResultTopType,
                CommunitiesResultSubType = CommunitiesResultSubType,
                ExportToVkAdsTargetGroup = ExportToVkAds
            };

            ApiCommandResult<ValidatedVkParsingTaskModalTm> remoteResult =
                await _apiRepository.ValidateVkParsingTaskModal(parsingTaskModalToValidate);

            if (remoteResult.Status == ApiCommandStatus.Ok)
            {
                ValidatedForm = remoteResult.Data;
                UpdateTitleFromValidatedModal();

                bool objectypeValid = ValidateObjectType();

                return objectypeValid ? true : false;
            }
            else
            {
                OnValidationFailure();

                return false;
            }
        }

        private void UpdateTitleFromValidatedModal()
        {
            if (AutoNamingIsOn)
            {
                _optionsSmTitleRawInput = ValidatedForm.ParsingTaskTitleSuggestion;
            }
        }

        private bool ValidateObjectType()
        { 
            if (ValidatedForm.SourcesObjectType != VkParsingTaskSourcesObjectType.Unknown)
            {
                return true;
            }
            else
            {
                OnValidationFailure();
                return false;
            }
        }

        private void OnValidationFailure()
        {
            SubmitButtonDisabled = true;
            ProfileResultTypeDisabled = true;
            CommunityResultTypeDisabled = true;

            ValidatedForm = new ValidatedVkParsingTaskModalTm();

            if (AutoNamingIsOn)
            {
                _optionsSmTitleRawInput = string.Empty;
            }
        }

        private void OnNoSourcesFound()
        {
            OptionsEnabled = false;

            OnValidationFailure();
        }

        private void HideAllChildOptions()
        {
            ResultTypeVisible = false;

            ProfilesResultSubTypeVisible = false;
            ProfilesResultTopTypeVisible = false;
            ProfilesResultActiveTypeVisible = false;
            ProfilesResultGroupIntersectionTypeVisible = false;
            ProfilesResultFriendsTypeVisible = false;

            CommunitiesResultSubTypeVisible = false;
            CommunitiesResultTaIntersectionTypeVisible = false;
            CommunitiesResultCommSearchTypeVisible = false;

            FilterSectionVisible = false;
            CommunityFilterOptionsVisible = false;
            AutomationSectionVisible = false;
            VkAdsExportSectionVisible = false;
            PeriodicTaskOptionsVisible = false;
        }

        private void ShowAllChildOptions()
        {
            ResultTypeVisible = true;
        }

        public bool ValidateUpdatedModalOptions()
        {
            if (ResultType == VkParsingTaskResultType.Profiles)
            {
                switch (ProfilesResultSubType)
                {
                    case VkParsingTaskResultProfilesSubType.Unknown:
                    default:
                        return false;
                    case VkParsingTaskResultProfilesSubType.All:
                        break;
                    case VkParsingTaskResultProfilesSubType.Top:
                        if (TopCommunitiesCount < 1 || ProfilesResultTopType == VkParsingTaskResultProfileTopType.Unknown)
                        {
                            return false;
                        }
                        break;
                    case VkParsingTaskResultProfilesSubType.Active:
                        bool valid = true;

                        if (ProfilesResultActiveSourcePosts == false && ProfilesResultActiveSourceDiscussions == false)
                        {
                            ActivitySourceInvalid = true;
                            valid = false;
                        }
                        else
                        {
                            ActivitySourceInvalid = false;
                        }

                        if (ProfilesResultActiveTypeComments == false &&
                            ProfilesResultActiveTypeLikes == false &&
                            ProfilesResultActiveTypeLikesInComments == false &&
                            ProfilesResultActiveTypePostAuthors == false &&
                            ProfilesResultActiveTypeReposts == false)
                        {
                            ActivityTypeInvalid = true;
                            valid = false;
                        }
                        else
                        {
                            ActivityTypeInvalid = false;
                        }

                        if (ProfilesResultActiveActivityCountFrom < 1)
                        {
                            valid = false;
                        }

                        if (!valid)
                        {
                            return valid;
                        }
                        break;
                    case VkParsingTaskResultProfilesSubType.GroupIntersection:
                        if (ProfilesResultGiCountFrom < 1 || ProfilesResultGiCountTo < 1)
                        {
                            return false;
                        }
                        break;
                    case VkParsingTaskResultProfilesSubType.Friends:
                        if (!ProfilesResultFriendsGetFriends.Value && !ProfilesResultFriendsGetFollowers.Value
                            && !ProfilesResultFriendsGetPeopleSubscriptions.Value)
                        {
                            FriendsTypeInvalid = true;
                            return false;
                        }
                        else
                        {
                            FriendsTypeInvalid = false;
                        }
                        break;
                    case VkParsingTaskResultProfilesSubType.DetailedProfiles:
                        break;
                }
            }

            if (ResultType == VkParsingTaskResultType.Communities)
            {
                switch (CommunitiesResultSubType)
                {
                    case VkParsingTaskResultCommunitiesSubType.Unknown:
                    default:
                        return false;
                    case VkParsingTaskResultCommunitiesSubType.TaIntersection:
                        if (CommunitiesResultTaIntersectionCommunitiesCount < 1 
                            || CommunitiesResultTaIntersectionTopType == VkParsingTaskResultCommunitiesTopType.Unknown)
                        {
                            return false;
                        }
                        break;
                    case VkParsingTaskResultCommunitiesSubType.CommunitiesSearch:
                        break;
                }
            }

            //if (ResultType == VkParsingTaskResultType.VkAdsExport)
            //{
            //    if (VkAdsAccount == null || VkAdsTargetGroup == null)
            //    {
            //        return false;
            //    }
            //}

            if (ResultType == VkParsingTaskResultType.Unknown)
            {
                return false;
            }

            if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.Top
                && (!TopCommunitiesCount.HasValue || TopCommunitiesCount.Value < 1))
            {
                return false;
            }

            if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.GroupIntersection
                && (!ProfilesResultGiCountFrom.HasValue || ProfilesResultGiCountFrom.Value < 1
                || !ProfilesResultGiCountTo.HasValue || ProfilesResultGiCountTo.Value < 1))
            {
                return false;
            }

            if (CommunitiesResultSubType == VkParsingTaskResultCommunitiesSubType.TaIntersection
                && (!CommunitiesResultTaIntersectionCommunitiesCount.HasValue 
                    || CommunitiesResultTaIntersectionCommunitiesCount.Value < 1))
            {
                return false;
            }

            if (CreatePeriodicTask)
            {
                if (PeriodicTaskExecutionRate == VkPeriodicParsingTaskRate.Unknown)
                {
                    return false;
                }
            }

            if (FilterEnabled)
            {
                if (LastCommWallPostPeriodStart == null || LastCommWallPostPeriodEnd == null)
                {
                    return false;
                }
            }

            if (ExportToVkAds)
            {
                if (VkAdsAccount == null 
                    || (VkAdsTargetGroup == null && CreateNewVkAdsTargetGroup == false)
                    || (VkAdsTargetGroup != null && CreateNewVkAdsTargetGroup == true))
                {
                    return false;
                }
            }

            return true;
        }

        //код дублируется, придумать что-то
        public virtual void LoadModel(VkOneTimeParsingTaskVm parsingTask)
        {
            _optionsSmTitleRawInput = parsingTask.Title;
            _optionsSmSourceType = parsingTask.SourceType;
            _optionsSmResultType = parsingTask.ResultType;
            _optionsSmRawLinkSources = parsingTask.Options.RawLinkSources;
            _optionsSmRawTaskSources = parsingTask.Options.RawTaskSources;
            _optionsSmSourceObjectsType = parsingTask.Options.SourcesObjectType;
            
            _optionsSmProfilesSubType = parsingTask.Options.ProfilesResultSubType;
            _optionsSmCommunitiesSubType = parsingTask.Options.CommunitiesResultSubType;
            _optionsSmUpdateActiveUsersTimeFrame = parsingTask.Options.UpdateActiveUsersTimeFrame;

            if (parsingTask.Options.TopProfilesOptions != null)
            {
                _optionsTopSmProfileTopType = parsingTask.Options.TopProfilesOptions.TopType;
                _optionsTopSmTopCommunitiesCount = parsingTask.Options.TopProfilesOptions.CommunitiesCount;
            }

            if (parsingTask.Options.ActiveProfilesOptions != null)
            {
                _optionsActiveSmSourcePosts = parsingTask.Options.ActiveProfilesOptions.ActivitySource.Posts;
                _optionsActiveSmSourceDiscussions = parsingTask.Options.ActiveProfilesOptions.ActivitySource.Discussions;
                _optionsActiveSmTypeComments = parsingTask.Options.ActiveProfilesOptions.ActivityType.Comments;
                _optionsActiveSmTypeLikes = parsingTask.Options.ActiveProfilesOptions.ActivityType.Likes;
                _optionsActiveSmTypeLikesInComments = parsingTask.Options.ActiveProfilesOptions.ActivityType.LikesInComments;
                _optionsActiveSmTypePostAuthors = parsingTask.Options.ActiveProfilesOptions.ActivityType.PostAuthors;
                _optionsActiveSmTypeReposts = parsingTask.Options.ActiveProfilesOptions.ActivityType.Reposts;
                _optionsActiveSmPeriodStart = parsingTask.Options.ActiveProfilesOptions.ActivityStartDateTime;
                _optionsActiveSmPeriodEnd = parsingTask.Options.ActiveProfilesOptions.ActivityEndDateTime;
                _optionsActiveSmActivityCountFrom = parsingTask.Options.ActiveProfilesOptions.ActivityCountFrom;
                _optionsActiveSmLimitWallPostsCount = parsingTask.Options.ActiveProfilesOptions.LimitWallPostsCount;
            }

            if (parsingTask.Options.GroupIntersectionProfilesOptions != null)
            {
                _optionsGroupIntersectionSmCountFrom = parsingTask.Options.GroupIntersectionProfilesOptions.CountFrom;
                _optionsGroupIntersectionSmCountTo = parsingTask.Options.GroupIntersectionProfilesOptions.CountTo;
            }

            if (parsingTask.Options.FriendsProfilesOptions != null)
            {
                _optionsFriendsSmGetFriends = parsingTask.Options.FriendsProfilesOptions.GetFriends;
                _optionsFriendsSmGetFollowers = parsingTask.Options.FriendsProfilesOptions.GetFollowers;
                _optionsFriendsSmGetPeopleSubscriptions = parsingTask.Options.FriendsProfilesOptions.GetPeopleSubscriptions;
            }

            if (parsingTask.Options.TaCommunitiesOptions != null)
            {
                _optionsSmCommunityTaIntersectionTopType = parsingTask.Options.TaCommunitiesOptions.TopType;
                _optionsSmCommunitiesTaIntersectionCommunitiesCount = parsingTask.Options.TaCommunitiesOptions.CommunitiesCount;
            }

            if (parsingTask.Options.CommunitiesSearchOptions != null)
            {
                _optionsSmCommunityCommSearchSearchType = parsingTask.Options.CommunitiesSearchOptions.SearchType;
                _optionsSmCommunityCommSearchGroupType = parsingTask.Options.CommunitiesSearchOptions.GroupType;
                _optionsSmCommunityCommSearchResultSort = parsingTask.Options.CommunitiesSearchOptions.ResultSort;
                _optionsSmCommunityCommSearchMarketOnly = parsingTask.Options.CommunitiesSearchOptions.Market;
                _optionsSmCommunityCommSearchMembersMin = parsingTask.Options.CommunitiesSearchOptions.MembersMin;
                _optionsSmCommunityCommSearchMembersMax = parsingTask.Options.CommunitiesSearchOptions.MembersMax;
                _optionsSmCommunityCommSearchPhraseSearch = parsingTask.Options.CommunitiesSearchOptions.PhraseSearch;
                _optionsSmCommunityCommSearchMinusWords = parsingTask.Options.CommunitiesSearchOptions.MinusWords;
                _optionsSmCommunityCommSearchTrending = parsingTask.Options.CommunitiesSearchOptions.Trending;
                _optionsSmCommunityCommSearchVerified = parsingTask.Options.CommunitiesSearchOptions.Verified;
            }

            _filterOptionsSmEnabled = parsingTask.FilterOptions.FilterEnabled;

            if (parsingTask.FilterOptions.CommunitiesFilterOptions != null)
            {
                _filterOptionsSmLastCommWallPostPeriodStart = parsingTask.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart;
                _filterOptionsSmLastCommWallPostPeriodEnd = parsingTask.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd;
            }

            _automationOptionsSmCreatePeriodicTask = parsingTask.AutomationOptions.CreatePeriodicTask;
            _automationOptionsSmPeriodicTaskExecutionRate = parsingTask.AutomationOptions.TaskExecutionRate;
            _vkAdsExportOptionsSmExportToVkAds = parsingTask.VkAdsExportOptions.ExportToVkAds;
            _vkAdsExportOptionsVmVkAdsAccount = parsingTask.VkAdsExportOptions.VkAdsAccount;
            _vkAdsExportOptionsVmVkAdsTargetGroup = parsingTask.VkAdsExportOptions.VkAdsTargetGroup;
        }

        public virtual void LoadModel(VkPeriodicParsingTaskVm parsingTask) 
        {
            _optionsSmTitleRawInput = parsingTask.Title;
            _optionsSmSourceType = parsingTask.SourceType;
            _optionsSmResultType = parsingTask.ResultType;
            _optionsSmRawLinkSources = parsingTask.Options.RawLinkSources;
            _optionsSmRawTaskSources = parsingTask.Options.RawTaskSources;
            _optionsSmSourceObjectsType = parsingTask.Options.SourcesObjectType;
            
            _optionsSmProfilesSubType = parsingTask.Options.ProfilesResultSubType;
            _optionsSmCommunitiesSubType = parsingTask.Options.CommunitiesResultSubType;
            _optionsSmUpdateActiveUsersTimeFrame = parsingTask.Options.UpdateActiveUsersTimeFrame;

            if (parsingTask.Options.TopProfilesOptions != null)
            {
                _optionsTopSmProfileTopType = parsingTask.Options.TopProfilesOptions.TopType;
                _optionsTopSmTopCommunitiesCount = parsingTask.Options.TopProfilesOptions.CommunitiesCount;
            }

            if (parsingTask.Options.ActiveProfilesOptions != null)
            {
                _optionsActiveSmSourcePosts = parsingTask.Options.ActiveProfilesOptions.ActivitySource.Posts;
                _optionsActiveSmSourceDiscussions = parsingTask.Options.ActiveProfilesOptions.ActivitySource.Discussions;
                _optionsActiveSmTypeComments = parsingTask.Options.ActiveProfilesOptions.ActivityType.Comments;
                _optionsActiveSmTypeLikes = parsingTask.Options.ActiveProfilesOptions.ActivityType.Likes;
                _optionsActiveSmTypeLikesInComments = parsingTask.Options.ActiveProfilesOptions.ActivityType.LikesInComments;
                _optionsActiveSmTypePostAuthors = parsingTask.Options.ActiveProfilesOptions.ActivityType.PostAuthors;
                _optionsActiveSmTypeReposts = parsingTask.Options.ActiveProfilesOptions.ActivityType.Reposts;
                _optionsActiveSmPeriodStart = parsingTask.Options.ActiveProfilesOptions.ActivityStartDateTime;
                _optionsActiveSmPeriodEnd = parsingTask.Options.ActiveProfilesOptions.ActivityEndDateTime;
                _optionsActiveSmActivityCountFrom = parsingTask.Options.ActiveProfilesOptions.ActivityCountFrom;
                _optionsActiveSmLimitWallPostsCount = parsingTask.Options.ActiveProfilesOptions.LimitWallPostsCount;
            }

            if (parsingTask.Options.GroupIntersectionProfilesOptions != null)
            {
                _optionsGroupIntersectionSmCountFrom = parsingTask.Options.GroupIntersectionProfilesOptions.CountFrom;
                _optionsGroupIntersectionSmCountTo = parsingTask.Options.GroupIntersectionProfilesOptions.CountTo;
            }

            if (parsingTask.Options.FriendsProfilesOptions != null)
            {
                _optionsFriendsSmGetFriends = parsingTask.Options.FriendsProfilesOptions.GetFriends;
                _optionsFriendsSmGetFollowers = parsingTask.Options.FriendsProfilesOptions.GetFollowers;
                _optionsFriendsSmGetPeopleSubscriptions = parsingTask.Options.FriendsProfilesOptions.GetPeopleSubscriptions;
            }

            if (parsingTask.Options.TaCommunitiesOptions != null)
            {
                _optionsSmCommunityTaIntersectionTopType = parsingTask.Options.TaCommunitiesOptions.TopType;
                _optionsSmCommunitiesTaIntersectionCommunitiesCount = parsingTask.Options.TaCommunitiesOptions.CommunitiesCount;
            }

            if (parsingTask.Options.CommunitiesSearchOptions != null)
            {
                _optionsSmCommunityCommSearchSearchType = parsingTask.Options.CommunitiesSearchOptions.SearchType;
                _optionsSmCommunityCommSearchGroupType = parsingTask.Options.CommunitiesSearchOptions.GroupType;
                _optionsSmCommunityCommSearchResultSort = parsingTask.Options.CommunitiesSearchOptions.ResultSort;
                _optionsSmCommunityCommSearchMarketOnly = parsingTask.Options.CommunitiesSearchOptions.Market;
                _optionsSmCommunityCommSearchMembersMin = parsingTask.Options.CommunitiesSearchOptions.MembersMin;
                _optionsSmCommunityCommSearchMembersMax = parsingTask.Options.CommunitiesSearchOptions.MembersMax;
                _optionsSmCommunityCommSearchPhraseSearch = parsingTask.Options.CommunitiesSearchOptions.PhraseSearch;
                _optionsSmCommunityCommSearchMinusWords = parsingTask.Options.CommunitiesSearchOptions.MinusWords;
                _optionsSmCommunityCommSearchTrending = parsingTask.Options.CommunitiesSearchOptions.Trending;
                _optionsSmCommunityCommSearchVerified = parsingTask.Options.CommunitiesSearchOptions.Verified;
            }

            _filterOptionsSmEnabled = parsingTask.FilterOptions.FilterEnabled;

            if (parsingTask.FilterOptions.CommunitiesFilterOptions != null)
            {
                _filterOptionsSmLastCommWallPostPeriodStart = parsingTask.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart;
                _filterOptionsSmLastCommWallPostPeriodEnd = parsingTask.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd;
            }

            _automationOptionsSmCreatePeriodicTask = parsingTask.AutomationOptions.CreatePeriodicTask;
            _automationOptionsSmPeriodicTaskExecutionRate = parsingTask.AutomationOptions.TaskExecutionRate;
            _vkAdsExportOptionsSmExportToVkAds = parsingTask.VkAdsExportOptions.ExportToVkAds;
            _vkAdsExportOptionsVmVkAdsAccount = parsingTask.VkAdsExportOptions.VkAdsAccount;
            _vkAdsExportOptionsVmVkAdsTargetGroup = parsingTask.VkAdsExportOptions.VkAdsTargetGroup;
        }

        private void UpdateOptionValuesOnResultTypeChange(VkParsingTaskResultType newValue)
        {
            if (newValue == VkParsingTaskResultType.Profiles)
            {
                _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Active;
                UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);

                _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;
                UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);

                _filterOptionsSmLastCommWallPostPeriodStart = DateTime.MinValue;
                _filterOptionsSmLastCommWallPostPeriodEnd = DateTime.MinValue;
            }

            if (newValue == VkParsingTaskResultType.Communities)
            {
                _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.TaIntersection;
                UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);

                _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
                UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);

                _vkAdsExportOptionsSmExportToVkAds = false;
                _vkAdsExportOptionsVmVkAdsAccount = null;
                _vkAdsExportOptionsVmVkAdsTargetGroup = null;
                _vkAdsExportOptionsCreateNewVkAdsTargetGroup = false;
                _vkAdsExportOptionsNewTargetGroupName = null;
            }

            if (newValue == VkParsingTaskResultType.VkAdsExport)
            {
                _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
                UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);

                _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;
                UpdateOptionValuesOnCommunitiesResultSubTypeChange(_optionsSmCommunitiesSubType);
            }
        }

        public void UpdateOptionValuesOnProfilesResultSubTypeChange(VkParsingTaskResultProfilesSubType newValue)
        {
            switch (newValue)
            {
                case VkParsingTaskResultProfilesSubType.Unknown:
                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;
                    _optionsActiveSmLimitWallPostsCount = null;

                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                case VkParsingTaskResultProfilesSubType.All:
                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;
                    _optionsActiveSmLimitWallPostsCount = null;

                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                case VkParsingTaskResultProfilesSubType.Top:
                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Top5;
                    _optionsTopSmTopCommunitiesCount = 1;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;

                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                case VkParsingTaskResultProfilesSubType.Active:
                    _optionsActiveSmSourcePosts = true;
                    _optionsActiveSmSourceDiscussions = false;
                    _optionsActiveSmTypeLikes = true;
                    _optionsActiveSmTypeLikesInComments = true;
                    _optionsActiveSmTypeComments = true;
                    _optionsActiveSmTypeReposts = false;
                    _optionsActiveSmTypePostAuthors = false;
                    _optionsActiveSmPeriodStart = DateTime.Now.AddMonths(-1);
                    _optionsActiveSmPeriodEnd = DateTime.Now;
                    _optionsActiveSmActivityCountFrom = 1;
                    _optionsActiveSmLimitWallPostsCount = true;

                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                case VkParsingTaskResultProfilesSubType.GroupIntersection:
                    _optionsGroupIntersectionSmCountFrom = 2;
                    _optionsGroupIntersectionSmCountTo = 2;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;
                    _optionsActiveSmLimitWallPostsCount = null;

                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                case VkParsingTaskResultProfilesSubType.Friends:
                    _optionsFriendsSmGetFriends = true;
                    _optionsFriendsSmGetFollowers = false;
                    _optionsFriendsSmGetPeopleSubscriptions = false;

                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;
                    _optionsActiveSmLimitWallPostsCount = null;

                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    break;

                case VkParsingTaskResultProfilesSubType.DetailedProfiles:
                    _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
                    _optionsTopSmTopCommunitiesCount = 0;

                    _optionsActiveSmSourcePosts = null;
                    _optionsActiveSmSourceDiscussions = null;
                    _optionsActiveSmTypeLikes = null;
                    _optionsActiveSmTypeLikesInComments = null;
                    _optionsActiveSmTypeComments = null;
                    _optionsActiveSmTypeReposts = null;
                    _optionsActiveSmTypePostAuthors = null;
                    _optionsActiveSmPeriodStart = DateTime.MinValue;
                    _optionsActiveSmPeriodEnd = DateTime.MinValue;
                    _optionsActiveSmActivityCountFrom = 0;
                    _optionsActiveSmLimitWallPostsCount = null;

                    _optionsGroupIntersectionSmCountFrom = 0;
                    _optionsGroupIntersectionSmCountTo = 0;

                    _optionsFriendsSmGetFriends = null;
                    _optionsFriendsSmGetFollowers = null;
                    _optionsFriendsSmGetPeopleSubscriptions = null;

                    break;

                default:
                    break;
            }
        }

        private void UpdateOptionValuesOnCommunitiesResultSubTypeChange(VkParsingTaskResultCommunitiesSubType newValue)
        {
            switch (newValue)
            {
                case VkParsingTaskResultCommunitiesSubType.Unknown:
                    _optionsSmCommunityTaIntersectionTopType = VkParsingTaskResultCommunitiesTopType.Unknown;
                    _optionsSmCommunitiesTaIntersectionCommunitiesCount = 0;

                    _optionsSmCommunityCommSearchSearchType = VkParsingTaskCommunitySearchSearchType.Unknown;
                    _optionsSmCommunityCommSearchGroupType = VkParsingTaskCommunitySearchCommType.Unknown;
                    _optionsSmCommunityCommSearchResultSort = VkParsingTaskCommunitySearchResultSort.Unknown;
                    _optionsSmCommunityCommSearchMarketOnly = false;
                    _optionsSmCommunityCommSearchMembersMin = null;
                    _optionsSmCommunityCommSearchMembersMax = null;
                    _optionsSmCommunityCommSearchPhraseSearch = false;
                    _optionsSmCommunityCommSearchMinusWords = null;
                    _optionsSmCommunityCommSearchTrending = false;
                    _optionsSmCommunityCommSearchVerified = false;

                    break;

                case VkParsingTaskResultCommunitiesSubType.TaIntersection:
                    _optionsSmCommunityTaIntersectionTopType = VkParsingTaskResultCommunitiesTopType.Top3;
                    _optionsSmCommunitiesTaIntersectionCommunitiesCount = 100;

                    _optionsSmCommunityCommSearchSearchType = VkParsingTaskCommunitySearchSearchType.Unknown;
                    _optionsSmCommunityCommSearchGroupType = VkParsingTaskCommunitySearchCommType.Unknown;
                    _optionsSmCommunityCommSearchResultSort = VkParsingTaskCommunitySearchResultSort.Unknown;
                    _optionsSmCommunityCommSearchMarketOnly = false;
                    _optionsSmCommunityCommSearchMembersMin = null;
                    _optionsSmCommunityCommSearchMembersMax = null;
                    _optionsSmCommunityCommSearchPhraseSearch = false;
                    _optionsSmCommunityCommSearchMinusWords = null;
                    _optionsSmCommunityCommSearchTrending = false;
                    _optionsSmCommunityCommSearchVerified = false;

                    break;

                case VkParsingTaskResultCommunitiesSubType.CommunitiesSearch:
                    _optionsSmCommunityCommSearchSearchType = VkParsingTaskCommunitySearchSearchType.Internal;
                    _optionsSmCommunityCommSearchGroupType = VkParsingTaskCommunitySearchCommType.Unknown;
                    _optionsSmCommunityCommSearchResultSort = VkParsingTaskCommunitySearchResultSort.Unknown;
                    _optionsSmCommunityCommSearchMarketOnly = false;
                    _optionsSmCommunityCommSearchMembersMin = null;
                    _optionsSmCommunityCommSearchMembersMax = null;
                    _optionsSmCommunityCommSearchPhraseSearch = false;
                    _optionsSmCommunityCommSearchMinusWords = null;
                    _optionsSmCommunityCommSearchTrending = false;
                    _optionsSmCommunityCommSearchVerified = false;

                    _optionsSmCommunityTaIntersectionTopType = VkParsingTaskResultCommunitiesTopType.Unknown;
                    _optionsSmCommunitiesTaIntersectionCommunitiesCount = 0;

                    break;

                default:
                    break;
            }
        }

        private void UpdateOptionValuesOnFilterEnabledChange(bool filterEnabled)
        {
            _filterOptionsSmLastCommWallPostPeriodStart = filterEnabled
                ? DateTime.Now.AddDays(-30)
                : DateTime.MinValue;
            _filterOptionsSmLastCommWallPostPeriodEnd = filterEnabled
                ? DateTime.Now 
                : DateTime.MinValue;
        }

        private void UpdateOptionValuesOnCreatePeriodicTaskChange(bool createPeriodicTask)
        {
            _automationOptionsSmPeriodicTaskExecutionRate = createPeriodicTask
                ? VkPeriodicParsingTaskRate.TwelveHours : null;
        }

        private void UpdateOptionValuesOnExportToVkAdsChange(bool exportToVkAds)
        {
            if (!exportToVkAds)
            {
                _vkAdsExportOptionsVmVkAdsAccount = null;
                _vkAdsExportOptionsVmVkAdsTargetGroup = null;
                _vkAdsExportOptionsCreateNewVkAdsTargetGroup = false;
                _vkAdsExportOptionsNewTargetGroupName = null;
                _vkAdsExportOptionsSmExportToVkAds = false;
            }
        }

        private DateTime SetLocalDateTimeForBrowserCalendarInput(DateTime input)
        {
            DateTime localInput = input.ToLocalTime();
            DateTime now = DateTime.Now;
            TimeSpan time = now.TimeOfDay;
            DateTime dateWithTime = localInput.Date + time;

            return dateWithTime;
        }

        public void UpdateTooltipTexts()
        {
            UpdateWhatToGetTooltipText();
            UpdateWhatProfilesToGetTooltipText();
            UpdateWhatCommunitiesToGetTooltipText();
        }

        private void UpdateWhatToGetTooltipText()
        {
            if (_optionsSmResultType == VkParsingTaskResultType.Profiles)
            {
                _whatToGetTooltipText = "В результате выполнения задачи вы получите список пользователей ВКонтакте, удовлетворяющих заданным критериям поиска. Вы сможете скачать пользователей, загрузить их в рекламный кабинет ВКонтакте или сделать источником для следующей задачи в цепочке выполнения.";
            }
            else if (_optionsSmResultType == VkParsingTaskResultType.Communities)
            {
                _whatToGetTooltipText = "В результате выполнения задачи вы получите список сообществ ВКонтакте, удовлетворяющих заданным критериям поиска. Вы сможете посмотреть детали найденных сообществ и сделать их источником для следующей задачи в цепочке выполнения.";
            }
            else
            {
                _whatToGetTooltipText = "Нет описания";
            }
        }

        private void UpdateWhatProfilesToGetTooltipText()
        {
            if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.All)
            {
                _whatUsersToGetTooltipText = "Сбор всех - вы получите всех участников указанных сообществ.";
            }
            else if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.Active)
            {
                if (_optionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.Group)
                {
                    _whatUsersToGetTooltipText = "Сбор активных из сообществ - вы получите только тех пользователей, кто проявил активность в указанных сообществах.";
                }
                else if (_optionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.User)
                {
                    _whatUsersToGetTooltipText = "Сбор активных из пользователей - вы получите только тех пользователей, кто проявил активность в указанных профилях.";
                }
            }
            else if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.Top)
            {
                _whatUsersToGetTooltipText = "Состоящие в ТОП - вы получите только тех участников сообществ, у кого данные группы находятся в топе интересных страниц.";
            }
            else if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.GroupIntersection)
            {
                if (_optionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.Group)
                {
                    _whatUsersToGetTooltipText = "Состоящие в 2-х и более - вы получите только тех участников указанных сообществ, которые состоят сразу в нескольких из них.";
                }
                else if (_optionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.User)
                {
                    _whatUsersToGetTooltipText = "Друзья у 2-х и более - вы получите только тех друзей указанных пользователей, которые являются друзьями у нескольких из них.";
                }
            }
            else if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.Friends)
            {
                _whatUsersToGetTooltipText = "Сбор Друзей - вы получите друзей указанных пользователей.";
            }
            else if (ProfilesResultSubType == VkParsingTaskResultProfilesSubType.DetailedProfiles)
            {
                _whatUsersToGetTooltipText = "Подробные профили - вы получите подробную информацию по указанным пользователям, которую можно экспортировать для детального анализа.";
            }
            else
            {
                _whatUsersToGetTooltipText = "Нет описания";
            }
        }

        private void UpdateWhatCommunitiesToGetTooltipText()
        {
            if (CommunitiesResultSubType == VkParsingTaskResultCommunitiesSubType.TaIntersection)
            {
                if (OptionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.Group)
                {
                    _whatCommunitiesToGetTooltipText = "Сообщества с ЦА - парсер соберёт сообщества с целевой аудиторией, анализируя ТОП-сообщества пользователей указанных сообществ.";
                }
                else if (OptionsSmSourceObjectsType == VkParsingTaskSourcesObjectType.User)
                {
                    _whatCommunitiesToGetTooltipText = "Сообщества с ЦА - парсер соберёт сообщества с целевой аудиторией, анализируя ТОП-сообщества указанных пользователей.";
                }
            }
            else if (CommunitiesResultSubType == VkParsingTaskResultCommunitiesSubType.CommunitiesSearch)
            {
                _whatCommunitiesToGetTooltipText = "Поиск сообществ - найдёт сообщества по указанным ключевым фразам.";
            }
            else
            {
                _whatCommunitiesToGetTooltipText = "Нет описания";
            }
        }

        private async Task PullAdsAccountsFromVkAsync()
        {
            ApiCommandResult<List<VkAdsAccountVm>> result = await _apiRepository.GetVkAdsAccounts();

            switch (result.Status)
            {
                case ApiCommandStatus.Ok:
                    _vkAdsAccounts = result.Data;
                    break;

                default:
                    break;
            }
        }

        private async Task PullAdsTargetGroupsOptionsFromVkAsync(VkAdsAccountVm adsAccount)
        {
            ApiCommandResult<List<VkAdsTargetGroupVm>> result = await _apiRepository.GetVkAdsTargetGroups(adsAccount.Id);

            switch (result.Status)
            {
                case ApiCommandStatus.Ok:
                    _vkAdsTargetGroups = result.Data;
                    break;

                default:
                    break;
            }
        }

        public async Task GetAvailableVkAdsParams()
        {
            if (ExportToVkAds && VkAdsAccount != null)
            {
                Task getCurrentAdsAccounts = PullAdsAccountsFromVkAsync();
                Task getCurrentAdsTargetGroups = PullAdsTargetGroupsOptionsFromVkAsync(VkAdsAccount);

                await Task.WhenAll(getCurrentAdsAccounts, getCurrentAdsTargetGroups);

                if (_vkAdsAccounts != null && VkAdsAccount != null)
                {
                    _vkAdsExportOptionsVmVkAdsAccount = _vkAdsAccounts.Where(a => a.Id == VkAdsAccount.Id).FirstOrDefault();

                    if (_vkAdsExportOptionsVmVkAdsAccount != null)
                    {
                        _vkAdsAccountId = _vkAdsExportOptionsVmVkAdsAccount.Id;
                    }
                }

                if (_vkAdsTargetGroups != null && VkAdsTargetGroup != null)
                {
                    _vkAdsExportOptionsVmVkAdsTargetGroup = _vkAdsTargetGroups.Where(a => a.Id == VkAdsTargetGroup.Id).FirstOrDefault();

                    if (_vkAdsExportOptionsVmVkAdsTargetGroup != null)
                    {
                        _vkAdsTargetGroupId = _vkAdsExportOptionsVmVkAdsTargetGroup.Id;
                    }
                }
            }
        }

        public async Task OnVkAdsExportChangedAsync(bool value)
        {
            if (value == true)
            {
                await PullAdsAccountsFromVkAsync();

                if (_vkAdsAccounts.Count == 1)
                {
                    await OnVkAdsAccountChangedAsync(_vkAdsAccounts.First().Id);
                }
            }
            else
            {
                _vkAdsAccountId = 0;
                _vkAdsTargetGroupId = 0;
            }
        }

        public async Task OnVkAdsAccountChangedAsync(long value)
        {
            _vkAdsAccountId = value;

            VkAdsAccountVm adsAccountVm = _vkAdsAccounts.Where(a => a.Id == value).FirstOrDefault();

            if (adsAccountVm != null)
            {
                VkAdsAccount = adsAccountVm;

                if (_vkAdsTargetGroups == null)
                {
                    await PullAdsTargetGroupsOptionsFromVkAsync(adsAccountVm);
                }
            }
            else
            {
                VkAdsAccount = null;
                _vkAdsTargetGroupId = 0;
                VkAdsTargetGroup = null;
            }
        }

        public void OnVkAdsTargetGroupChanged(long value)
        {
            _vkAdsTargetGroupId = value;

            if (_vkAdsTargetGroupId == -1) //создать новую
            {
                CreateNewVkAdsTargetGroup = true;
                VkAdsTargetGroup = null;
            }
            else
            {
                CreateNewVkAdsTargetGroup = false;
                NewTargetGroupName = null;

                VkAdsTargetGroupVm selectedAdsTargetGroup = _vkAdsTargetGroups
                    .Where(e => e.Id == value).FirstOrDefault();

                VkAdsTargetGroup = selectedAdsTargetGroup ?? null;
            }
        }

        private async Task OnVkAdsExportChangedOnExportTaskAsync(bool value)
        {
            if (value == true)
            {
                if (_vkAdsAccounts == null)
                {
                    await PullAdsAccountsFromVkAsync();

                    if (_vkAdsAccounts.Count == 1)
                    {
                        await SetVkAdsAccountAsync(_vkAdsAccounts.First().Id);
                    }
                }
            }
            else
            {
                _vkAdsAccountId = 0;
            }
        }

        private async Task SetVkAdsAccountAsync(long value)
        {
            _vkAdsAccountId = value;

            VkAdsAccountVm adsAccountVm = _vkAdsAccounts.Where(a => a.Id == value).FirstOrDefault();

            if (adsAccountVm != null)
            {
                _vkAdsExportOptionsVmVkAdsAccount = adsAccountVm;
                VkAdsTargetGroupsSelectorVisible = true;

                if (_vkAdsTargetGroups == null)
                {
                    await PullAdsTargetGroupsOptionsFromVkAsync(adsAccountVm);
                }
            }
            else
            {
                _vkAdsExportOptionsVmVkAdsAccount = null;
                _vkAdsTargetGroupId = 0;
                _vkAdsExportOptionsVmVkAdsTargetGroup = null;
                VkAdsTargetGroupsSelectorVisible = false;
            }
        }

        public async Task OnVkAdsAccountChangedOnExportTaskAsync(long value)
        {
            _vkAdsAccountId = value;

            VkAdsAccountVm adsAccountVm = _vkAdsAccounts.Where(a => a.Id == value).FirstOrDefault();

            if (adsAccountVm != null)
            {
                _vkAdsExportOptionsVmVkAdsAccount = adsAccountVm;

                if (_vkAdsTargetGroups == null)
                {
                    await PullAdsTargetGroupsOptionsFromVkAsync(adsAccountVm);
                }
            }
            else
            {
                _vkAdsExportOptionsVmVkAdsAccount = null;
                _vkAdsTargetGroupId = 0;
                _vkAdsExportOptionsVmVkAdsTargetGroup = null;

                _vkAdsExportOptionsCreateNewVkAdsTargetGroup = false;
                _vkAdsExportOptionsNewTargetGroupName = null;
            }
        }

        public void OnVkAdsTargetGroupChangedOnExportTask(long value)
        {
            _vkAdsTargetGroupId = value;

            if (_vkAdsTargetGroupId == -1) //создать новую
            {
                _vkAdsExportOptionsCreateNewVkAdsTargetGroup = true;
                _vkAdsExportOptionsVmVkAdsTargetGroup = null;
            }
            else
            {
                _vkAdsExportOptionsCreateNewVkAdsTargetGroup = false;
                _vkAdsExportOptionsNewTargetGroupName = null;

                VkAdsTargetGroupVm selectedAdsTargetGroup = _vkAdsTargetGroups
                    .Where(e => e.Id == value).FirstOrDefault();

                _vkAdsExportOptionsVmVkAdsTargetGroup = selectedAdsTargetGroup ?? null;
            }
        }

        public void OnNewTargetGroupNameChangedOnExportTask(string value)
        {
            _vkAdsExportOptionsNewTargetGroupName = value;
        }

        public void OnTaskNameChangedOnExportTask(string value)
        {
            _optionsSmTitleRawInput = value;
        }

        public void AddTasksAsSources(ICollection<VkQuickParsingTaskVm> selectedTasks, bool useBackingField)
        {
            StringBuilder sb = new StringBuilder();

            foreach (VkQuickParsingTaskVm item in selectedTasks)
            {
                sb.AppendLine(item.YaVkParsingTaskID.ToString());
            }

            if (useBackingField)
            {
                _optionsSmRawTaskSources = sb.ToString();
            }
            else
            {
                OptionsSmRawTaskSources = sb.ToString();
            }
        }

        public async Task SetExportToVkAdsOnExportTask(bool value)
        {
            _vkAdsExportOptionsSmExportToVkAds = value;
            
            await OnVkAdsExportChangedOnExportTaskAsync(value);
        }

        public void SetPeriodicParsingTaskOptionsDisabled(bool value)
        {
            _periodicTaskOptionsSmDisabled = value;
        }


        public virtual void Reset()
        {
            SubmitButtonDisabled = true;

            ValidatedForm = new ValidatedVkParsingTaskModalTm();

            ResultTypeVisible = false;

            ProfilesResultSubTypeVisible = false;
            ProfilesResultTopTypeVisible = false;
            ProfilesResultActiveTypeVisible = false;
            ProfilesResultGroupIntersectionTypeVisible = false;
            ProfilesResultFriendsTypeVisible = false;

            CommunitiesResultSubTypeVisible = false;
            CommunitiesResultTaIntersectionTypeVisible = false;
            CommunitiesResultCommSearchTypeVisible = false;

            FilterSectionVisible = false;
            CommunityFilterOptionsVisible = false;

            PeriodicTaskOptionsVisible = false;
            AutomationSectionVisible = false;
            VkAdsExportSectionVisible = false;
            VkAdsAccountSelectorVisible = false;
            VkAdsTargetGroupsSelectorVisible = false;
            NewTargetGroupNameSelectorVisible = false;

            ProfileResultTypeDisabled = true;
            CommunityResultTypeDisabled = true;

            ProfilesResultAllTypeDisabled = true;
            ProfilesResultTopTypeDisabled = true;
            ProfilesResultActiveTypeDisabled = true;
            ProfilesResultGroupIntersectionTypeDisabled = true;
            ProfilesResultFriendsTypeDisabled = true;
            ProfilesResultDetailedProfilesTypeDisabled = true;
            CommunitiesResultTaIntersectionTypeDisabled = true;
            CommunitiesResultCommSearchTypeDisabled = true;

            PeriodicTaskOptionsDisabled = false;

            SourcesCountInvalid = false;
            ActivitySourceInvalid = false;
            ActivityTypeInvalid = false;
            FriendsTypeInvalid = false;

            SkipBlazoriseValidation = false;

            _optionsEnabled = false;

            _autoNamingIsOn = true;

            _optionsSmTitleRawInput = string.Empty;
            _optionsSmRawLinkSources = null;
            _optionsSmRawTaskSources = null;
            _optionsSmSourceObjectsType = VkParsingTaskSourcesObjectType.Unknown;

            _optionsSmSourceType = VkParsingTaskSourceType.Unknown;
            _optionsSmResultType = VkParsingTaskResultType.Unknown;
            _optionsSmCommunitiesSubType = VkParsingTaskResultCommunitiesSubType.Unknown;

            _optionsTopSmProfileTopType = VkParsingTaskResultProfileTopType.Unknown;
            _optionsTopSmTopCommunitiesCount = 0;

            _optionsSmCommunityTaIntersectionTopType = VkParsingTaskResultCommunitiesTopType.Unknown;
            _optionsSmCommunitiesTaIntersectionCommunitiesCount = 0;

            _optionsSmCommunityCommSearchSearchType = VkParsingTaskCommunitySearchSearchType.Unknown;
            _optionsSmCommunityCommSearchGroupType = VkParsingTaskCommunitySearchCommType.Unknown;
            _optionsSmCommunityCommSearchResultSort = VkParsingTaskCommunitySearchResultSort.Unknown;
            _optionsSmCommunityCommSearchMarketOnly = false;
            _optionsSmCommunityCommSearchMembersMin = null;
            _optionsSmCommunityCommSearchMembersMax = null;
            _optionsSmCommunityCommSearchPhraseSearch = false;
            _optionsSmCommunityCommSearchMinusWords = null;
            _optionsSmCommunityCommSearchTrending = false;
            _optionsSmCommunityCommSearchVerified = false;

            _optionsActiveSmSourcePosts = null;
            _optionsActiveSmSourceDiscussions = null;
            _optionsActiveSmTypeLikes = null;
            _optionsActiveSmTypeLikesInComments = null;
            _optionsActiveSmTypeComments = null;
            _optionsActiveSmTypeReposts = null;
            _optionsActiveSmTypePostAuthors = null;
            _optionsActiveSmPeriodStart = DateTime.MinValue;
            _optionsActiveSmPeriodEnd = DateTime.MinValue;
            _optionsActiveSmActivityCountFrom = 0;
            _optionsActiveSmLimitWallPostsCount = null;

            _optionsGroupIntersectionSmCountFrom = 0;
            _optionsGroupIntersectionSmCountTo = 0;

            _optionsFriendsSmGetFriends = null;
            _optionsFriendsSmGetFollowers = null;
            _optionsFriendsSmGetPeopleSubscriptions = null;

            _filterOptionsSmEnabled = false;
            _filterOptionsSmLastCommWallPostPeriodStart = DateTime.MinValue;
            _filterOptionsSmLastCommWallPostPeriodEnd = DateTime.MinValue;

            _automationOptionsSmCreatePeriodicTask = false;
            _automationOptionsSmPeriodicTaskExecutionRate = null;
            _vkAdsExportOptionsSmExportToVkAds = false;
            _vkAdsExportOptionsVmVkAdsAccount = null;
            _vkAdsExportOptionsVmVkAdsTargetGroup = null;
            _vkAdsExportOptionsCreateNewVkAdsTargetGroup = false;
            _vkAdsExportOptionsNewTargetGroupName = null;
            
            _vkAdsAccounts = null;
            _vkAdsTargetGroups = null;
            _vkAdsAccountId = 0;
            _vkAdsTargetGroupId = 0;

            _whatToGetTooltipText = string.Empty;
            _whatUsersToGetTooltipText = string.Empty;
            _whatCommunitiesToGetTooltipText = string.Empty;
        }

        public void SetDefaultLinksSourceOptions()
        {
            _optionsSmSourceType = VkParsingTaskSourceType.Links;
            _optionsSmResultType = VkParsingTaskResultType.Profiles;
            UpdateOptionValuesOnResultTypeChange(_optionsSmResultType);
            _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Active;
            UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
        }

        public void SetDefaultTasksSourceOptions()
        {
            _optionsSmSourceType = VkParsingTaskSourceType.Tasks;
            _optionsSmResultType = VkParsingTaskResultType.Profiles;
            UpdateOptionValuesOnResultTypeChange(_optionsSmResultType);
            _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Active;
            UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);
        }

        public void SetDefaultVkAdsExportOptions(string title)
        {
            _optionsSmSourceType = VkParsingTaskSourceType.Tasks;
            _optionsSmResultType = VkParsingTaskResultType.VkAdsExport;
            UpdateOptionValuesOnResultTypeChange(_optionsSmResultType);
            _optionsSmProfilesSubType = VkParsingTaskResultProfilesSubType.Unknown;
            UpdateOptionValuesOnProfilesResultSubTypeChange(_optionsSmProfilesSubType);

            _optionsSmTitleRawInput = title;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _modalInputTimer.Dispose();
                }

                _modalInputTimer = null;

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
