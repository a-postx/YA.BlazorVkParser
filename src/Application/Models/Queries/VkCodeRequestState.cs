using System;

namespace YA.WebClient.Application.Models.Queries
{
    [Serializable]
    public class VkCodeRequestState
    {
        public string ClientRequestId { get; set; }
        public string RedirectAddress { get; set; }
    }
}
