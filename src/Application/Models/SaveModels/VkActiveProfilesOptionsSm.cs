namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки парсинга для типа результата "Профили-Активные".
/// </summary>
public class VkActiveProfilesOptionsSm : ValueObject
{
    private VkActiveProfilesOptionsSm() { }

    public VkActiveProfilesOptionsSm(DateTime startDateTime,
        DateTime endDateTime,
        int activityCountFrom,
        VkActivitySourceOptionsSm activitySource,
        VkActivityTypeOptionsSm activityType,
        bool limitWallPostsCount)
    {
        ActivityStartDateTime = startDateTime;
        ActivityEndDateTime = endDateTime;
        ActivityCountFrom = activityCountFrom;
        ActivitySource = activitySource;
        ActivityType = activityType;
        LimitWallPostsCount = limitWallPostsCount;
    }

    /// <summary>
    /// Дата начала периода активностию.
    /// </summary>
    public DateTime ActivityStartDateTime { get; private set; }

    /// <summary>
    /// Дата конца периода активностию.
    /// </summary>
    public DateTime ActivityEndDateTime { get; private set; }

    /// <summary>
    /// Минимальное количество активностей.
    /// </summary>
    public int ActivityCountFrom { get; private set; }

    /// <summary>
    /// Настройки источников активностей.
    /// </summary>
    public VkActivitySourceOptionsSm ActivitySource { get; private set; }

    /// <summary>
    /// Настройки типов активностей
    /// </summary>
    public VkActivityTypeOptionsSm ActivityType { get; private set; }

    /// <summary>
    /// Признак ограничения количества постов 1000 шт.
    /// </summary>
    public bool LimitWallPostsCount { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ActivityStartDateTime;
        yield return ActivityEndDateTime;
        yield return ActivityCountFrom;
        yield return ActivitySource;
        yield return ActivityType;
        yield return LimitWallPostsCount;
    }
}
