using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Рекламный кабинет ВКонтакте, модель сохранения.
    /// </summary>
    public class VkAdsAccountSm : ValueObject
    {
        private VkAdsAccountSm() { }

        public VkAdsAccountSm(long accountId, string accountName)
        {
            Id = accountId;
            Name = accountName;
        }

        /// <summary>
        /// Идентификатор рекламного кабинета.
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// Имя рекламного кабинета.
        /// </summary>
        public string Name { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
        }
    }
}