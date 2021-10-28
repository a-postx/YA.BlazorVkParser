namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Профили-Пересечения".
    /// </summary>
    public class VkGroupIntersectionProfilesOptionsVm
    {
        /// <summary>
        /// Минимальное количество вхождений в группы.
        /// </summary>
        public int CountFrom { get; set; }

        /// <summary>
        /// Максимально количество вхождений в группы.
        /// </summary>
        public int CountTo { get; set; }
    }
}
