namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Идентификаторы профилей участников ВК сообщества, визуальная модель.
/// </summary>
public class VkCommunityProfileIdsVm
{
    /// <summary>
    /// Общее число идентификаторов.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Массив идентификаторов.
    /// </summary>
    public ICollection<long> ProfileIds { get; set; }
}
