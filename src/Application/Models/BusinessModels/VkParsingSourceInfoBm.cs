using YA.WebClient.Application.Enums;

namespace YA.WebClient.Application.Models.BusinessModels
{
    public class VkParsingSourceInfoBm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public VkParsingSourcePageType Type { get; set; }
    }
}
