namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки парсинга для типа результата "Сообщества-ПоискЦА".
/// </summary>
public class VkTaCommunitiesOptionsVm
{
    /// <summary>
    /// Тип результата сбора сообществ с учётом топа интересных страниц.
    /// </summary>
    public VkParsingTaskResultCommunitiesTopType TopType { get; set; }

    /// <summary>
    /// Число сообществ в результате анализа.
    /// </summary>
    public int CommunitiesCount { get; set; }
}
