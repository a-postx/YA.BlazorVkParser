using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки фильтрации одоразовой задачи парсинга ВКонтакте, модель сохранения.
    /// </summary>
    public class VkParsingTaskFilterOptionsSm : ValueObject
    {
        private VkParsingTaskFilterOptionsSm() { }

        public VkParsingTaskFilterOptionsSm(bool filterEnabled, VkCommunitiesFilterOptionsSm communitiesFilterOptions)
        {
            FilterEnabled = filterEnabled;
            CommunitiesFilterOptions = communitiesFilterOptions;
        }

        /// <summary>
        /// Признак необходимости фильтрации результатов задачи.
        /// </summary>
        public bool FilterEnabled { get; private set; }
        /// <summary>
        /// Фильтр результатов для типа результата "Сообщества".
        /// </summary>
        public VkCommunitiesFilterOptionsSm CommunitiesFilterOptions { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FilterEnabled;
            yield return CommunitiesFilterOptions;
        }
    }
}
