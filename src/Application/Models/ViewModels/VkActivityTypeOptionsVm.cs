namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки типов активностей
/// </summary>
public class VkActivityTypeOptionsVm
{
    /// <summary>
    /// Лайки
    /// </summary>
    public bool Likes { get; set; }

    /// <summary>
    /// Необходимо зачислить в Активные Пользователи лайкающих в комментариях
    /// </summary>
    public bool LikesInComments { get; set; }

    /// <summary>
    /// Комментарии
    /// </summary>
    public bool Comments { get; set; }

    /// <summary>
    /// Репосты
    /// </summary>
    public bool Reposts { get; set; }

    /// <summary>
    /// Авторы постов
    /// </summary>
    public bool PostAuthors { get; set; }
}
