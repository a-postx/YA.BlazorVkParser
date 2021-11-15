namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Группа аудитории ВКонтакте, визуальная модель.
/// </summary>
public class VkAdsTargetGroupVm
{
    /// <summary>
    /// Идентификатор группы.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Название группы.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Количество аудитории.
    /// </summary>
    public long? AudienceCount { get; set; }
}
