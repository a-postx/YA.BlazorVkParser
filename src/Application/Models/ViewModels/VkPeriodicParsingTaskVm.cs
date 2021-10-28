using System;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Периодическая задача парсинга ВК после создания, визуальная модель.
    /// </summary>
    public class VkPeriodicParsingTaskVm
    {
        /// <summary>
        /// Статус задачи, заданный пользователем.
        /// </summary>
        public VkPeriodicParsingTaskExecutionOptions ExecutionOption { get; set; }

        /// <summary>
        /// Дата следующего запуска.
        /// </summary>
        public DateTime? NextExecutionDateTime { get; set; }

        //Десериализация родительских классов не поддерживается (обещают завезти в в5), поэтому пока без наследования

        /// <summary>
        /// Уникальный идентификатор задачи.
        /// </summary>
        public Guid YaVkParsingTaskID { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Тип источника входящих данных.
        /// </summary>
        public VkParsingTaskSourceType SourceType { get; set; }

        /// <summary>
        /// Тип результата задачи.
        /// </summary>
        public VkParsingTaskResultType ResultType { get; set; }

        /// <summary>
        /// Настройки задачи.
        /// </summary>
        public VkParsingTaskOptionsVm Options { get; set; }

        /// <summary>
        /// Настройки фильтрации задачи.
        /// </summary>
        public VkParsingTaskFilterOptionsVm FilterOptions { get; set; }

        /// <summary>
        /// Настройки автоматизации задачи.
        /// </summary>
        public VkParsingTaskAutomationOptionsVm AutomationOptions { get; set; }

        /// <summary>
        /// Настройки экспорта результата задачи в рекламный кабинет ВКонтакте.
        /// </summary>
        public VkParsingTaskVkAdsExportOptionsVm VkAdsExportOptions { get; set; }

        /// <summary>
        /// Адрес созданной сущности, согласно спецификации RESTful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Статус состояния задачи.
        /// </summary>
        public VkParsingTaskOperationStatus OperationStatus { get; set; }

        /// <summary>
        /// Результат процессинга задачи.
        /// </summary>
        public VkParsingTaskProcessingResult ProcessingResult { get; set; }

        /// <summary>
        /// Время выполнения задачи в миллисекундах.
        /// </summary>
        public long? ExecutionTime { get; set; }

        /// <summary>
        /// Процент выполнения задачи.
        /// </summary>
        public int? ExecutionPercentCompleted { get; set; }

        /// <summary>
        /// Количество ВК результатов.
        /// </summary>
        public int? VkontakteResultsCount { get; set; }

        /// <summary>
        /// Количество Инстаграм результатов.
        /// </summary>
        public int? InstagramResultsCount { get; set; }

        /// <summary>
        /// Признак наличия результата с идентификаторами профилей ВКонтакте.
        /// </summary>
        public bool VkProfileIdsResultsExist { get; set; }

        /// <summary>
        /// Признак наличия результата со ссылками профилей ВКонтакте.
        /// </summary>
        public bool VkProfileLinksResultsLinkExist { get; set; }

        /// <summary>
        /// Признак наличия результата с подробными профилями ВКонтакте в CSV.
        /// </summary>
        public bool VkProfileCsvResultsLinkExist { get; set; }

        /// <summary>
        /// Признак наличия результата с профилями Инстаграм.
        /// </summary>
        public bool IgProfileResultsLinkExist { get; set; }

        /// <summary>
        /// Признак наличия результата со ссылками профилей Инстаграм.
        /// </summary>
        public bool IgProfileLinksResultsLinkExist { get; set; }

        /// <summary>
        /// Дата создания задачи.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Дата последней модификации задачи.
        /// </summary>
        public DateTime LastModifiedDateTime { get; set; }
    }
}
