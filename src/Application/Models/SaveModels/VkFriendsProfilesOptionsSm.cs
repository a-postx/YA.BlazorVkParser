using System.Collections.Generic;

namespace YA.WebClient.Application.Models.SaveModels
{
    /// <summary>
    /// Настройки парсинга для типа результата "Профили-Друзья".
    /// </summary>
    public class VkFriendsProfilesOptionsSm : ValueObject
    {
        private VkFriendsProfilesOptionsSm() { }
        public VkFriendsProfilesOptionsSm(bool getFriends, bool getFollowers, bool getPeopleSubscriptions)
        {
            GetFriends = getFriends;
            GetFollowers = getFollowers;
            GetPeopleSubscriptions = getPeopleSubscriptions;
        }

        /// <summary>
        /// Собрать друзей
        /// </summary>
        public bool GetFriends { get; private set; }
        /// <summary>
        /// Собрать подписчиков
        /// </summary>
        public bool GetFollowers { get; private set; }
        /// <summary>
        /// Собрать профили, на которые подписан
        /// </summary>
        public bool GetPeopleSubscriptions { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return GetFriends;
            yield return GetFollowers;
            yield return GetPeopleSubscriptions;
        }
    }
}
