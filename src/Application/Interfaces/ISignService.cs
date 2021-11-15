namespace YA.WebClient.Application.Interfaces;

public interface ISignService
{
    void BeginLogin(string returnAddress = "");
    Task BeginLogoutAsync(string returnAddress = "", CancellationToken cancellationToken = default);
}
