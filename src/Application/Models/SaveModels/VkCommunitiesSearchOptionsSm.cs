using System.Collections.Generic;
using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки поиска групп ВКонтакте, модель сохранения.
    /// </summary>
    public class VkCommunitiesSearchOptionsSm : ValueObject
    {
        public VkCommunitiesSearchOptionsSm(VkParsingTaskCommunitySearchSearchType searchType,
            VkParsingTaskCommunitySearchCommType groupType, VkParsingTaskCommunitySearchResultSort resultSort, bool market,
            int? membersMin, int? membersMax, bool phraseSearch, string minusWords, bool trending, bool verified)
        {
            SearchType = searchType;
            GroupType = groupType;
            ResultSort = resultSort;
            Market = market;
            MembersMin = membersMin;
            MembersMax = membersMax;
            PhraseSearch = phraseSearch;
            MinusWords = minusWords;
            Trending = trending;
            Verified = verified;
        }

        /// <summary>
        /// Тип поиска
        /// </summary>
        public VkParsingTaskCommunitySearchSearchType SearchType { get; private set; }

        /// <summary>
        /// Тип групп для поиска
        /// </summary>
        public VkParsingTaskCommunitySearchCommType GroupType { get; private set; }

        /// <summary>
        /// Параметр сортировки результатов поиска
        /// </summary>
        public VkParsingTaskCommunitySearchResultSort ResultSort { get; private set; }

        /// <summary>
        /// Признак поиска сообществ только с товарами
        /// </summary>
        public bool Market { get; private set; }

        /// <summary>
        /// Минимальное количество участников
        /// </summary>
        public int? MembersMin { get; private set; }

        /// <summary>
        /// Максимальное количество участников
        /// </summary>
        public int? MembersMax { get; private set; }

        /// <summary>
        /// Признак поиска по точному вхождению фраз
        /// </summary>
        public bool PhraseSearch { get; private set; }

        /// <summary>
        /// Минус-слова
        /// </summary>
        public string MinusWords { get; private set; }

        /// <summary>
        /// Признак поиска только сообществ с Прометеем
        /// </summary>
        public bool Trending { get; private set; }

        /// <summary>
        /// Признак поиска только верифицированных сообществ
        /// </summary>
        public bool Verified { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SearchType;
            yield return GroupType;
            yield return ResultSort;
            yield return Market;
            yield return MembersMin;
            yield return MembersMax;
            yield return PhraseSearch;
            yield return MinusWords;
            yield return Trending;
            yield return Verified;
        }
    }
}
