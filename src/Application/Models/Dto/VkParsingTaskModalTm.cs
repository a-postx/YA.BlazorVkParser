namespace YA.WebClient.Application.Models.Dto;

/// <summary>
/// Параметры окна задачи парсинга ВКонтакте, модель передачи.
/// </summary>
public class VkParsingTaskModalTm
{
    /// <summary>
    /// Тип источника данных.
    /// </summary>
    public VkParsingTaskSourceType SourceType { get; set; }

    /// <summary>
    /// Введённый текст источников парсинга ВКонтакте.
    /// </summary>
    public string RawLinkSources { get; set; }

    /// <summary>
    /// Источники парсинга - идентификаторы выполненных одноразовых задач.
    /// </summary>
    public string RawTaskSources { get; set; }

    /// <summary>
    /// Тип получаемого результата.
    /// </summary>
    public VkParsingTaskResultType ResultType { get; set; }

    /// <summary>
    /// Тип результата сбора профилей.
    /// </summary>
    public VkParsingTaskResultProfilesSubType ProfilesResultSubType { get; set; }

    /// <summary>
    /// Тип результата сбора сообществ.
    /// </summary>
    public VkParsingTaskResultCommunitiesSubType CommunitiesResultSubType { get; set; }

    /// <summary>
    /// Тип результата сбора профилей с учётом топа интересных страниц.
    /// </summary>
    public VkParsingTaskResultProfileTopType? ProfilesResultTopType { get; set; }

    /// <summary>
    /// Признак экспорта результатов в рекламный кабинет ВКонтакте.
    /// </summary>
    public bool ExportToVkAdsTargetGroup { get; set; }
}
