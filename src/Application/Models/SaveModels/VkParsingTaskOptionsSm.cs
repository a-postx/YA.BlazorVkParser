using System.Collections.Generic;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки задачи парсинга ВКонтакте, модель сохранения.
    /// </summary>
    public class VkParsingTaskOptionsSm : ValueObject
    {
        private VkParsingTaskOptionsSm() { }

        public VkParsingTaskOptionsSm(string rawLinkSources,
            string rawTaskSources,
            VkParsingTaskSourcesObjectType sourcesObjectType,
            VkParsingTaskResultProfilesSubType profilesResultSubType,
            VkParsingTaskResultCommunitiesSubType communitiesResultSubType,
            VkTopProfilesOptionsSm topProfilesOptions,
            VkActiveProfilesOptionsSm activeProfilesOptions,
            VkGroupIntersectionProfilesOptionsSm groupIntersectionOptions,
            VkFriendsProfilesOptionsSm friendsOptions,
            VkTaCommunitiesOptionsSm taCommunitiesOptions,
            VkCommunitiesSearchOptionsSm commSearchOptions,
            bool updateActiveUsersDate)
        {
            RawLinkSources = rawLinkSources;
            RawTaskSources = rawTaskSources;
            SourcesObjectType = sourcesObjectType;
            ProfilesResultSubType = profilesResultSubType;
            CommunitiesResultSubType = communitiesResultSubType;
            TopProfilesOptions = topProfilesOptions;
            ActiveProfilesOptions = activeProfilesOptions;
            GroupIntersectionProfilesOptions = groupIntersectionOptions;
            FriendsProfilesOptions = friendsOptions;
            TaCommunitiesOptions = taCommunitiesOptions;
            CommunitiesSearchOptions = commSearchOptions;
            UpdateActiveUsersTimeFrame = updateActiveUsersDate;
        }

        /// <summary>
        /// Идентификатор(ы) исходных данных, введённые пользователем.
        /// </summary>
        public string RawLinkSources { get; private set; }

        /// <summary>
        /// Источники парсинга - идентификаторы выполненных одноразовых задач.
        /// </summary>
        public string RawTaskSources { get; private set; }

        /// <summary>
        /// Тип объектов в источниках.
        /// </summary>
        public VkParsingTaskSourcesObjectType SourcesObjectType { get; private set; }

        /// <summary>
        /// Тип результата сбора профилей.
        /// </summary>
        public VkParsingTaskResultProfilesSubType ProfilesResultSubType { get; private set; }

        /// <summary>
        /// Тип результата сбора сообществ.
        /// </summary>
        public VkParsingTaskResultCommunitiesSubType CommunitiesResultSubType { get; private set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Топ".
        /// </summary>
        public VkTopProfilesOptionsSm TopProfilesOptions { get; private set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Активные".
        /// </summary>
        public VkActiveProfilesOptionsSm ActiveProfilesOptions { get; private set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Пересечения".
        /// </summary>
        public VkGroupIntersectionProfilesOptionsSm GroupIntersectionProfilesOptions { get; private set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Друзья".
        /// </summary>
        public VkFriendsProfilesOptionsSm FriendsProfilesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Сообщества-ПоискЦА".
        /// </summary>
        public VkTaCommunitiesOptionsSm TaCommunitiesOptions { get; private set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Сообщества-ПоискСообществ".
        /// </summary>
        public VkCommunitiesSearchOptionsSm CommunitiesSearchOptions { get; private set; }

        /// <summary>
        /// Признак сдвига дат при повторном сборе активной аудитории.
        /// </summary>
        public bool UpdateActiveUsersTimeFrame { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RawLinkSources;
            yield return RawTaskSources;
            yield return ProfilesResultSubType;
            yield return CommunitiesResultSubType;
            yield return TopProfilesOptions;
            yield return ActiveProfilesOptions;
            yield return GroupIntersectionProfilesOptions;
            yield return FriendsProfilesOptions;
            yield return TaCommunitiesOptions;
            yield return CommunitiesSearchOptions;
            yield return UpdateActiveUsersTimeFrame;
        }
    }
}
