namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки выгрузки в рекламный кабинет ВКонтакте, модель сохранения.
/// </summary>
public class VkParsingTaskVkAdsExportOptionsSm : ValueObject
{
    private VkParsingTaskVkAdsExportOptionsSm() { }

    public VkParsingTaskVkAdsExportOptionsSm(bool exportToVkAds,
        VkAdsAccountSm vkAdsAccount,
        VkAdsTargetGroupSm vkAdsTargetGroup,
        bool? createNewTargetGroup,
        string newTargetGroupName)
    {
        ExportToVkAds = exportToVkAds;
        VkAdsAccount = vkAdsAccount;
        VkAdsTargetGroup = vkAdsTargetGroup;
        CreateNewTargetGroup = createNewTargetGroup;
        NewTargetGroupName = newTargetGroupName;
    }

    /// <summary>
    /// Отправлять результаты в рекламный кабинет ВКонтакте.
    /// </summary>
    public bool ExportToVkAds { get; private set; }
    /// <summary>
    /// Рекламный кабинет.
    /// </summary>
    public VkAdsAccountSm VkAdsAccount { get; private set; }
    /// <summary>
    /// Целевая группа аудитории.
    /// </summary>
    public VkAdsTargetGroupSm VkAdsTargetGroup { get; private set; }
    /// <summary>
    /// Признак создания целевой группы аудитории.
    /// </summary>
    public bool? CreateNewTargetGroup { get; private set; }
    /// <summary>
    /// Имя новой группы аудитории.
    /// </summary>
    public string NewTargetGroupName { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ExportToVkAds;
        yield return VkAdsAccount;
        yield return VkAdsTargetGroup;
        yield return CreateNewTargetGroup;
        yield return NewTargetGroupName;
    }
}
