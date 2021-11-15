namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки сообщества ВКонтакте, модель сохранения.
/// </summary>
public class VkCommunityOptionsSm
{
    /// <summary>
    /// Собирать информацию о членах сообщества.
    /// </summary>
    public bool GetCommunityProfiles { get; set; }
    /// <summary>
    /// Собирать информацию о позиции сообщества в топе у профиля.
    /// </summary>
    public bool GetCommunityPlacementInProfiles { get; set; }
    /// <summary>
    /// Собирать информацию о постах на стене сообщества.
    /// </summary>
    public bool GetCommunityWallPosts { get; set; }
    /// <summary>
    /// Собирать информацию о комментарих в постах сообщества.
    /// </summary>
    public bool GetCommunityWallPostComments { get; set; }
    /// <summary>
    /// Собирать информацию о постах на стене у членов сообщества.
    /// </summary>
    public bool GetCommunityMemberProfileWallPosts { get; set; }
    /// <summary>
    /// Создать задачу для автоматического обновления информации. По-умолчанию, одноразовая операция.
    /// </summary>
    public bool ScheduleForRegularUpdate { get; set; }
}
