namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Статистика сообщества ВКонтакте.
/// </summary>
public class VkCommunityStatisticsVm
{
    /// <summary>
    /// Дата последнего поста в сообществе.
    /// </summary>
    public DateTime? LastPostDate { get; set; }
    /// <summary>
    /// Просматриваемость.
    /// </summary>
    public int? Viewability { get; set; }
    /// <summary>
    /// Количество постов в день.
    /// </summary>
    public double? PostsPerDay { get; set; }
    /// <summary>
    /// Количество просмотров на пост.
    /// </summary>
    public double? ViewsPerPost { get; set; }
    /// <summary>
    /// Количество вовлечений в день.
    /// </summary>
    public double? ErDay { get; set; }
    /// <summary>
    /// Количество вовлечений на пост.
    /// </summary>
    public double? ErPost { get; set; }
}
