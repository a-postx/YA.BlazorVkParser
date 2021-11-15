namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// ВК сообщество после создания, визуальная модель.
/// </summary>
public class VkCommunityCreateVm
{
    /// <summary>
    /// Уникальный идентификатор сообщества.
    /// </summary>
    public Guid YaVkCommunityID { get; set; }

    /// <summary>
    /// Идентификатор сообщества Вконтакте.
    /// </summary>
    public long VkCommunityId { get; set; }

    /// <summary>
    /// Адрес созданной сущности, согласно спецификации RESTful JSON http://restfuljson.org/.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// Статус процессинга сообщества.
    /// </summary>
    public VkCommunityOperationStatus OperationStatus { get; set; }
}
