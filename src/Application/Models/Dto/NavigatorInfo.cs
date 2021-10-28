namespace YA.WebClient.Application.Models.Dto
{
    public class NavigatorInfo
    {
        public string AppCodeName { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string Vendor { get; set; }
        public string VendorSub { get; set; }
        public string Product { get; set; }
        public string ProductSub { get; set; }
        public string Platform { get; set; }
        public string OsCpu { get; set; }
        public bool CookieEnabled { get; set; }
        public string Language { get; set; }
        public string UserAgent { get; set; }
    }
}
