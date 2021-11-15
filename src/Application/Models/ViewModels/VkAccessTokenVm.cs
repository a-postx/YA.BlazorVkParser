namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Токен доступа ВКонтакте, визуальная модель.
/// </summary>
public class VkAccessTokenVm
{
    /// <summary>
    /// ВК-идентификатор пользователя, от которого получен доступ.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Токен активен и пригоден к выполнению задач.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Время последней модификации объекта.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }
}
