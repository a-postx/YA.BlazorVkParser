namespace YA.WebClient.Application.Models.ViewModels;

public class InvitationVm
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; }
    public Guid YaInvitationID { get; set; }
    public string InvitedBy { get; set; }
    public string Email { get; set; }
    public MembershipAccessType AccessType { get; set; }
    public bool Claimed { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public TenantInvitationStatus Status { get; set; }
    public DateTime CreatedDateTime { get; set; }
}
