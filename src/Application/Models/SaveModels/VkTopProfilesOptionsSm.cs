namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки парсинга для типа результата "Профили-Топ".
/// </summary>
public class VkTopProfilesOptionsSm : ValueObject
{
    private VkTopProfilesOptionsSm() { }

    public VkTopProfilesOptionsSm(VkParsingTaskResultProfileTopType topType,
        int topCount)
    {
        TopType = topType;
        CommunitiesCount = topCount;
    }

    /// <summary>
    /// Тип результата сбора профилей с учётом топа интересных страниц.
    /// </summary>
    public VkParsingTaskResultProfileTopType TopType { get; private set; }

    /// <summary>
    /// Число сообществ, которые у пользователя в топе.
    /// </summary>
    public int CommunitiesCount { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return TopType;
        yield return CommunitiesCount;
    }
}
