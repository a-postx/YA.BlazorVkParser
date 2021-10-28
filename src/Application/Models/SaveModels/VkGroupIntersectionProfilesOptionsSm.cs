using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Профили-Пересечения".
    /// </summary>
    public class VkGroupIntersectionProfilesOptionsSm : ValueObject
    {
        private VkGroupIntersectionProfilesOptionsSm() { }
        public VkGroupIntersectionProfilesOptionsSm(int countFrom, int countTo)
        {
            CountFrom = countFrom;
            CountTo = countTo;
        }

        /// <summary>
        /// Число общих сообществ (от)
        /// </summary>
        public int CountFrom { get; private set; }

        /// <summary>
        /// Число общих сообществ (до)
        /// </summary>
        public int CountTo { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CountFrom;
            yield return CountTo;
        }
    }
}
