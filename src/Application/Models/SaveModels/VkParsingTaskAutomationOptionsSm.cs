using System.Collections.Generic;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки автоматизации одоразовой задачи парсинга ВКонтакте, модель сохранения.
    /// </summary>
    public class VkParsingTaskAutomationOptionsSm : ValueObject
    {
        private VkParsingTaskAutomationOptionsSm() { }

        public VkParsingTaskAutomationOptionsSm(bool createPeriodicTask,
            VkPeriodicParsingTaskRate? taskExecutionRate)
        {
            CreatePeriodicTask = createPeriodicTask;
            TaskExecutionRate = taskExecutionRate;
        }

        /// <summary>
        /// Создать также периодическую задачу.
        /// </summary>
        public bool CreatePeriodicTask { get; private set; }
        /// <summary>
        /// Периодичность запуска периодической задачи.
        /// </summary>
        public VkPeriodicParsingTaskRate? TaskExecutionRate { get; private set; }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CreatePeriodicTask;
            yield return TaskExecutionRate;
        }
    }
}
