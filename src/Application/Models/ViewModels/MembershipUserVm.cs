namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Пользователь приложения, визуальная модель.
/// </summary>
public class MembershipUserVm
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid UserID { get; set; }
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Электропочта.
    /// </summary>
    public string Email { get; set; }
}
