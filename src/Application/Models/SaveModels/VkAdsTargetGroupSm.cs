using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Группа аудитории ВКонтакте, модель сохранения.
    /// </summary>
    public class VkAdsTargetGroupSm : ValueObject
    {
        private VkAdsTargetGroupSm() { }

        public VkAdsTargetGroupSm(long id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// Название группы.
        /// </summary>
        public string Name { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
        }
    }
}
