namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки поиска групп ВКонтакте, модель сохранения.
/// </summary>
public class VkCommunitiesSearchOptionsVm
{
    /// <summary>
    /// Тип поиска
    /// </summary>
    public VkParsingTaskCommunitySearchSearchType SearchType { get; set; }

    /// <summary>
    /// Тип сообществ
    /// </summary>
    public VkParsingTaskCommunitySearchCommType GroupType { get; set; }

    /// <summary>
    /// Параметр сортировки результатов поиска
    /// </summary>
    public VkParsingTaskCommunitySearchResultSort ResultSort { get; set; }

    /// <summary>
    /// Признак поиска сообществ только с товарами
    /// </summary>
    public bool Market { get; set; }

    /// <summary>
    /// Минимальное количество участников
    /// </summary>
    public int? MembersMin { get; set; }

    /// <summary>
    /// Максимальное количество участников
    /// </summary>
    public int? MembersMax { get; set; }

    /// <summary>
    /// Признак поиска по точному вхождению фраз
    /// </summary>
    public bool PhraseSearch { get; set; }

    /// <summary>
    /// Минус-слова
    /// </summary>
    public string MinusWords { get; set; }

    /// <summary>
    /// Признак поиска только сообществ с Прометеем
    /// </summary>
    public bool Trending { get; set; }

    /// <summary>
    /// Признак поиска только верифицированных сообществ
    /// </summary>
    public bool Verified { get; set; }
}
