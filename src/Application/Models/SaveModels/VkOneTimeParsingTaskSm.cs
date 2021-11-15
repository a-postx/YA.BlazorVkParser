namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Одноразовая задача парсинга ВКонтакте, модель сохранения.
/// </summary>
public class VkOneTimeParsingTaskSm : ValueObject
{
    public VkOneTimeParsingTaskSm(string title,
        VkParsingTaskSourceType sourceType,
        VkParsingTaskResultType resultType,
        bool autodelete,
        VkParsingTaskOptionsSm options,
        VkParsingTaskFilterOptionsSm filterOptions,
        VkParsingTaskAutomationOptionsSm automationOptions,
        VkParsingTaskVkAdsExportOptionsSm vkAdsExportOptions)
    {
        Title = title;
        SourceType = sourceType;
        ResultType = resultType;
        Autodelete = autodelete;
        Options = options;
        FilterOptions = filterOptions;
        AutomationOptions = automationOptions;
        VkAdsExportOptions = vkAdsExportOptions;
    }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; private set; }
    /// <summary>
    /// Тип источника входящих данных.
    /// </summary>
    public VkParsingTaskSourceType SourceType { get; private set; }
    /// <summary>
    /// Тип результата задачи.
    /// </summary>
    public VkParsingTaskResultType ResultType { get; private set; }
    /// <summary>
    /// Автоудаление задачи.
    /// </summary>
    public bool Autodelete { get; private set; }
    /// <summary>
    /// Настройки задачи.
    /// </summary>
    public VkParsingTaskOptionsSm Options { get; private set; }
    /// <summary>
    /// Настройки фильтрации задачи.
    /// </summary>
    public VkParsingTaskFilterOptionsSm FilterOptions { get; private set; }
    /// <summary>
    /// Настройки автоматизации задачи.
    /// </summary>
    public VkParsingTaskAutomationOptionsSm AutomationOptions { get; private set; }
    /// <summary>
    /// Настройки автоматизации задачи.
    /// </summary>
    public VkParsingTaskVkAdsExportOptionsSm VkAdsExportOptions { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Title;
        yield return SourceType;
        yield return ResultType;
        yield return Autodelete;
        yield return Options;
        yield return FilterOptions;
        yield return AutomationOptions;
        yield return VkAdsExportOptions;
    }
}
