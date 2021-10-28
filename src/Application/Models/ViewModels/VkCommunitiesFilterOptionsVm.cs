using System;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Сообщества-Фильтр".
    /// </summary>
    public class VkCommunitiesFilterOptionsVm
    {
        /// <summary>
        /// Дата начала интервала последнего поста.
        /// </summary>
        public DateTime LastPostPeriodStart { get; set; }

        /// <summary>
        /// Дата конца интервала последнего поста.
        /// </summary>
        public DateTime LastPostPeriodEnd { get; set; }
    }
}
