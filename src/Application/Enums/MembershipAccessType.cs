namespace YA.WebClient.Application.Enums;

[Flags]
public enum MembershipAccessType
{
    None = 0,
    ReadOnly = 1,
    ReadWrite = 2,
    Admin = 4,
    Owner = 8
}
