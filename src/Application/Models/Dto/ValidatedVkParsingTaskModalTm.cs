using System;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.Dto
{
    [Flags]
    public enum EnabledProfileParsers
    {
        None = 0,
        All = 1,
        Active = 2,
        Top = 4,
        GroupIntersection = 8,
        Friends = 16,
        DetailedProfiles = 32
    }

    [Flags]
    public enum EnabledCommunityParsers
    {
        None = 0,
        TaIntersectionSearch = 1,
        CommunitySearch = 2
    }

    /// <summary>
    /// Проверенная форма задачи парсинга ВКонтакте, модель передачи.
    /// </summary>
    public class ValidatedVkParsingTaskModalTm
    {
        /// <summary>
        /// Тип объектов в источниках.
        /// </summary>
        public VkParsingTaskSourcesObjectType SourcesObjectType { get; set; }

        /// <summary>
        /// Доступные варианты парсинга для поиска профилей ВКонтакте.
        /// </summary>
        public EnabledProfileParsers EnabledProfileParsers { get; set; }

        /// <summary>
        /// Доступные варианты парсинга для поиска сообществ ВКонтакте.
        /// </summary>
        public EnabledCommunityParsers EnabledCommunityParsers { get; set; }

        /// <summary>
        /// Предлагаемое название задачи парсинга.
        /// </summary>
        public string ParsingTaskTitleSuggestion { get; set; }
    }
}
