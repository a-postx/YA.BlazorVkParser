using System;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Задача парсинга ВК для результатов быстрого поиска, визуальная модель.
    /// </summary>
    public class VkQuickParsingTaskVm
    {
        /// <summary>
        /// Уникальный идентификатор задачи.
        /// </summary>
        public Guid YaVkParsingTaskID { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Количество результатов в задаче.
        /// </summary>
        public int? VkResultsCount { get; set; }

        /// <summary>
        /// Тип результатов в задаче.
        /// </summary>
        public VkParsingTaskResultType ResultType { get; set; }
    }
}
