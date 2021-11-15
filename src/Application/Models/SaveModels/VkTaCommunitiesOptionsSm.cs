namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки парсинга для типа результата "Сообщества-ПоискЦА".
/// </summary>
public class VkTaCommunitiesOptionsSm : ValueObject
{
    private VkTaCommunitiesOptionsSm() { }

    public VkTaCommunitiesOptionsSm(VkParsingTaskResultCommunitiesTopType topType, int communitiesCount)
    {
        TopType = topType;
        CommunitiesCount = communitiesCount;
    }

    /// <summary>
    /// Тип результата сбора сообществ с учётом топа интересных страниц.
    /// </summary>
    public VkParsingTaskResultCommunitiesTopType TopType { get; set; }

    /// <summary>
    /// Число сообществ в результате анализа.
    /// </summary>
    public int CommunitiesCount { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return TopType;
        yield return CommunitiesCount;
    }
}
