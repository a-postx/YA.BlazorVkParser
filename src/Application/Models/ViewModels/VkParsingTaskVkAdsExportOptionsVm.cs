namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки экспорта в рекламный кабинет ВКонтакте
/// </summary>
public class VkParsingTaskVkAdsExportOptionsVm
{
    /// <summary>
    /// Отправлять результаты в рекламный кабинет ВКонтакте.
    /// </summary>
    public bool ExportToVkAds { get; set; }
    /// <summary>
    /// Рекламный кабинет.
    /// </summary>
    public VkAdsAccountVm VkAdsAccount { get; set; }
    /// <summary>
    /// Целевая группа аудитории.
    /// </summary>
    public VkAdsTargetGroupVm VkAdsTargetGroup { get; set; }
    /// <summary>
    /// Признак создания целевой группы аудитории.
    /// </summary>
    public bool? CreateNewTargetGroup { get; set; }
    /// <summary>
    /// Имя новой группы аудитории.
    /// </summary>
    public string NewTargetGroupName { get; set; }
}
