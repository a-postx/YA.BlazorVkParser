namespace YA.WebClient.Application.Interfaces;

/// <summary>
/// Хранилище использованных кодов аутентификации ВКонтакте,
/// используем для предотвращения повторного использования и ошибок с ним связанных
/// </summary>
public interface IVkOauthCodesState
{
    ICollection<string> UsedCodes { get; set; }
}
