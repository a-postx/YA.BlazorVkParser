using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Настройки автоматизации одоразовой задачи парсинга ВКонтакте, визуальная модель.
    /// </summary>
    public class VkParsingTaskAutomationOptionsVm
    {
        /// <summary>
        /// Создать также периодическую задачу.
        /// </summary>
        public bool CreatePeriodicTask { get; set; }
        /// <summary>
        /// Периодичность запуска периодической задачи.
        /// </summary>
        public VkPeriodicParsingTaskRate? TaskExecutionRate { get; set; }
    }
}
