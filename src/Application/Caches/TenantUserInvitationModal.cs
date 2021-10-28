using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Caches
{
    public class TenantUserInvitationModal
    {
        public string Email { get; set; }
        public MembershipAccessType AccessType { get; set; } = MembershipAccessType.ReadOnly;
        public string InvitedBy { get; set; }
    }
}
