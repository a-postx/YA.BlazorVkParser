using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки задачи парсинга ВКонтакте, визуальная модель.
    /// </summary>
    public class VkParsingTaskOptionsVm
    {
        /// <summary>
        /// Источники парсинга - ссылки на сообщества или пользователей.
        /// </summary>
        public string RawLinkSources { get; set; }

        /// <summary>
        /// Источники парсинга - идентификаторы выполненных одноразовых задач.
        /// </summary>
        public string RawTaskSources { get; set; }

        /// <summary>
        /// Тип объектов в источниках.
        /// </summary>
        public VkParsingTaskSourcesObjectType SourcesObjectType { get; set; }

        /// <summary>
        /// Тип результата сбора профилей.
        /// </summary>
        public VkParsingTaskResultProfilesSubType ProfilesResultSubType { get; set; }

        /// <summary>
        /// Тип результата сбора сообществ.
        /// </summary>
        public VkParsingTaskResultCommunitiesSubType CommunitiesResultSubType { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Топ".
        /// </summary>
        public VkTopProfilesOptionsVm TopProfilesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Активные", визуальная модель.
        /// </summary>
        public VkActiveProfilesOptionsVm ActiveProfilesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Пересечения".
        /// </summary>
        public VkGroupIntersectionProfilesOptionsVm GroupIntersectionProfilesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Профили-Друзья".
        /// </summary>
        public VkFriendsProfilesOptionsVm FriendsProfilesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Сообщества-ПоискЦА".
        /// </summary>
        public VkTaCommunitiesOptionsVm TaCommunitiesOptions { get; set; }

        /// <summary>
        /// Настройки парсинга для типа результата "Сообщества-ПоискСообществ".
        /// </summary>
        public VkCommunitiesSearchOptionsVm CommunitiesSearchOptions { get; set; }

        /// <summary>
        /// Признак сдвига дат при повторном сборе активной аудитории.
        /// </summary>
        public bool UpdateActiveUsersTimeFrame { get; set; }
    }
}
