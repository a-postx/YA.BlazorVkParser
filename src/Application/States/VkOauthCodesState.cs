namespace YA.WebClient.Application.States;

public class VkOauthCodesState : IVkOauthCodesState
{
    public ICollection<string> UsedCodes { get; set; } = new List<string>();
}
