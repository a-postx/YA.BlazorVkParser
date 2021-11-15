namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Пользователь, модель сохранения.
/// </summary>
public class UserSm : ValueObject
{
    public UserSm(string name, string email, UserSettingSm settings)
    {
        Name = name;
        Email = email;
        Settings = settings;
    }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Электропочта пользователя.
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// Настройки пользователя.
    /// </summary>
    public UserSettingSm Settings { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
        yield return Email;
        yield return Settings;
    }
}
