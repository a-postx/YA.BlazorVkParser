namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Приглашение в арендатора, модель сохранения.
/// </summary>
public class InvitationSm : ValueObject
{
    public InvitationSm(string email, MembershipAccessType accessType, string invitedBy)
    {
        Email = email;
        AccessType = accessType;
        InvitedBy = invitedBy;
    }

    /// <summary>
    /// Почтовый адрес приглашения.
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// Тип доступа приглашения.
    /// </summary>
    public MembershipAccessType AccessType { get; private set; }
    /// <summary>
    /// Пригласивший пользователь.
    /// </summary>
    public string InvitedBy { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Email;
        yield return AccessType;
        yield return InvitedBy;
    }
}
