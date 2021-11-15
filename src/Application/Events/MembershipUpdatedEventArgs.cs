namespace YA.WebClient.Application.Events;

public class MembershipUpdatedEventArgs : EventArgs
{
    public MembershipVm Membership { get; set; }
}
