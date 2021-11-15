namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Пользователь приложения, визуальная модель.
/// </summary>
public class UserVm
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
    /// <summary>
    /// Настройки.
    /// </summary>
    public UserSettingVm Settings { get; set; }
    /// <summary>
    /// Членство пользователя в арендаторах.
    /// </summary>
    public ICollection<MembershipVm> Memberships { get; set; }
    /// <summary>
    /// Арендаторы, в которых пользователь имеет членство.
    /// </summary>
    public ICollection<TenantVm> Tenants { get; set; }
}
