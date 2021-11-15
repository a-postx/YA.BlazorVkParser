namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Членство пользователя, визуальная модель.
/// </summary>
public class MembershipVm
{
    /// <summary>
    /// Идентификатор членства.
    /// </summary>
    public Guid MembershipID { get; set; }
    /// <summary>
    /// Пользователь.
    /// </summary>
    public MembershipUserVm User { get; set; }
    /// <summary>
    /// Арендатор.
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Тип доступа пользователя к арендатору.
    /// </summary>
    public MembershipAccessType AccessType { get; set; }
    /// <summary>
    /// Дата создания членства.
    /// </summary>
    public DateTime CreatedDateTime { get; set; }
}
