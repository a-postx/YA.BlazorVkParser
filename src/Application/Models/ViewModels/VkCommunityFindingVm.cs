namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Найденная группа ВКонтакте, визуальная модель.
    /// </summary>
    public class VkCommunityFindingVm
    {
        /// <summary>
        /// Идентификатор ВКонтакте.
        /// </summary>
        public long VkCommunityId { get; set; }
        /// <summary>
        /// Название группы.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Адрес картинки группы.
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// Количество членов группы.
        /// </summary>
        public int MembersCount { get; set; }
        /// <summary>
        /// Общая статистика группы.
        /// </summary>
        public VkCommunityStatisticsVm Statistics { get; set; }
        /// <summary>
        /// Количество членов исходных сообществ (при поиске похожей аудитории).
        /// </summary>
        public int? SourceCommunitiesMembersCount { get; set; }
        /// <summary>
        /// Признак выбранного пользователем результата.
        /// </summary>
        public bool Selected { get; set; }
    }
}
