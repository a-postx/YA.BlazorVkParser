namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Сообщество ВКонтакте, модель сохранения.
    /// </summary>
    public class VkCommunitySm
    {
        /// <summary>
        /// Идентификатор сообщества ВК.
        /// </summary>
        public long VkCommunityId { get; set; }
        /// <summary>
        /// Настройки сообщества.
        /// </summary>
        public VkCommunityOptionsSm Options { get; set; }
    }
}
