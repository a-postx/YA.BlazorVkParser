namespace YA.WebClient.Application.Interfaces;

public interface IClientInfoService
{
    Task<(ClientInfoVm, Guid)> PublishClientInfoAsync();
}
