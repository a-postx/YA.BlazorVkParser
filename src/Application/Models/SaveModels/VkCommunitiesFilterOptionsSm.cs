using System;
using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки фильтрации задачи парсинга ВКонтакте, модель сохранения.
    /// </summary>
    public class VkCommunitiesFilterOptionsSm : ValueObject
    {
        private VkCommunitiesFilterOptionsSm() { }

        public VkCommunitiesFilterOptionsSm(DateTime lastPostPeriodStart, DateTime lastPostPeriodEnd)
        {
            LastPostPeriodStart = lastPostPeriodStart;
            LastPostPeriodEnd = lastPostPeriodEnd;
        }

        /// <summary>
        /// Дата начала интервала последнего поста.
        /// </summary>
        public DateTime LastPostPeriodStart { get; private set; }

        /// <summary>
        /// Дата конца интервала последнего поста.
        /// </summary>
        public DateTime LastPostPeriodEnd { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LastPostPeriodStart;
            yield return LastPostPeriodEnd;
        }
    }
}
