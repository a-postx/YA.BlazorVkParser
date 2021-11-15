namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки парсинга для типа результата "Профили-Друзья".
/// </summary>
public class VkFriendsProfilesOptionsVm
{
    /// <summary>
    /// Собрать друзей
    /// </summary>
    public bool GetFriends { get; set; }
    /// <summary>
    /// Собрать подписчиков
    /// </summary>
    public bool GetFollowers { get; set; }
    /// <summary>
    /// Собрать профили, на которые подписан
    /// </summary>
    public bool GetPeopleSubscriptions { get; set; }
}
