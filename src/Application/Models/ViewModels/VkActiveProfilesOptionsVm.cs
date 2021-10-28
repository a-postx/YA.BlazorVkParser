using System;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Профили-Активные".
    /// </summary>
    public class VkActiveProfilesOptionsVm
    {
        /// <summary>
        /// Дата начала периода активностию.
        /// </summary>
        public DateTime ActivityStartDateTime { get; set; }

        /// <summary>
        /// Дата конца периода активности.
        /// </summary>
        public DateTime ActivityEndDateTime { get; set; }

        /// <summary>
        /// Минимальное количество активностей.
        /// </summary>
        public int ActivityCountFrom { get; set; }

        /// <summary>
        /// Настройки источников активностей.
        /// </summary>
        public VkActivitySourceOptionsVm ActivitySource { get; set; }

        /// <summary>
        /// Настройки типов активностей
        /// </summary>
        public VkActivityTypeOptionsVm ActivityType { get; set; }

        /// <summary>
        /// Признак ограничения количества постов 1000 шт.
        /// </summary>
        public bool LimitWallPostsCount { get; set; }
    }
}
