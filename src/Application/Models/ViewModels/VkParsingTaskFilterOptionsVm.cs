namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки фильтрации результатов задачи.
/// </summary>
public class VkParsingTaskFilterOptionsVm
{
    /// <summary>
    /// Признак необходимости фильтрации результатов задачи.
    /// </summary>
    public bool FilterEnabled { get; set; }
    /// <summary>
    /// Фильтр результатов для типа результата "Сообщества".
    /// </summary>
    public VkCommunitiesFilterOptionsVm CommunitiesFilterOptions { get; set; }
}
