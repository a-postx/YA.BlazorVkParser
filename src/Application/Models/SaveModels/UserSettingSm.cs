namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки пользователя, модель сохранения.
/// </summary>
public class UserSettingSm : ValueObject
{
    public UserSettingSm(bool showGettingStarted)
    {
        ShowGettingStarted = showGettingStarted;
    }

    /// <summary>
    /// Признак необходимости показа страницы регистрации.
    /// </summary>
    public bool ShowGettingStarted { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ShowGettingStarted;
    }
}
