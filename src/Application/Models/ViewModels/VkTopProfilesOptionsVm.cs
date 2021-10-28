using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Профили-Топ", визуальная модель.
    /// </summary>
    public class VkTopProfilesOptionsVm
    {
        /// <summary>
        /// Тип результата сбора профилей с учётом топа интересных страниц.
        /// </summary>
        public VkParsingTaskResultProfileTopType TopType { get; set; }

        /// <summary>
        /// Число сообществ, которые у пользователя в топе.
        /// </summary>
        public int CommunitiesCount { get; set; }
    }
}
