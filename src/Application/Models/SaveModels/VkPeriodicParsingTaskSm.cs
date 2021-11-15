namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Периодическая задача парсинга ВКонтакте, модель сохранения.
/// </summary>
public class VkPeriodicParsingTaskSm : ValueObject
{
    private VkPeriodicParsingTaskSm() { }

    public VkPeriodicParsingTaskSm(string title,
        VkParsingTaskResultType resultType,
        VkParsingTaskOptionsSm options,
        VkParsingTaskFilterOptionsSm filterOptions,
        VkParsingTaskAutomationOptionsSm automationOptions,
        VkParsingTaskVkAdsExportOptionsSm vkAdsExportOptions,
        VkPeriodicParsingTaskExecutionOptions executionOption)
    {
        Title = title;
        ResultType = resultType;
        Options = options;
        FilterOptions = filterOptions;
        AutomationOptions = automationOptions;
        VkAdsExportOptions = vkAdsExportOptions;
        ExecutionOption = executionOption;
    }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Тип результата задачи.
    /// </summary>
    public VkParsingTaskResultType ResultType { get; private set; }

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

    /// <summary>
    /// Статус запуска задачи, заданный пользователем.
    /// </summary>
    public VkPeriodicParsingTaskExecutionOptions ExecutionOption { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Title;
        yield return ResultType;
        yield return Options;
        yield return FilterOptions;
        yield return AutomationOptions;
        yield return VkAdsExportOptions;
        yield return ExecutionOption;
    }
}
