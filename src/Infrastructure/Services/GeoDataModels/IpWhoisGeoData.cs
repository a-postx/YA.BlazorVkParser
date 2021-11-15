namespace YA.WebClient.Infrastructure.Services.GeoDataModels;

public class IpWhoisGeoData
{
#pragma warning disable IDE1006 // Naming Styles
    public string ip { get; set; }
    public bool success { get; set; }
    public string type { get; set; }
    public string continent { get; set; }
    public string continent_code { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }
    public string country_flag { get; set; }
    public string country_capital { get; set; }
    public string country_phone { get; set; }
    public string country_neighbours { get; set; }
    public string region { get; set; }
    public string city { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string asn { get; set; }
    public string org { get; set; }
    public string isp { get; set; }
    public string timezone { get; set; }
    public string timezone_name { get; set; }
    public string timezone_dstOffset { get; set; }
    public string timezone_gmtOffset { get; set; }
    public string timezone_gmt { get; set; }
    public string currency { get; set; }
    public string currency_code { get; set; }
    public string currency_symbol { get; set; }
    public string currency_rates { get; set; }
    public string currency_plural { get; set; }
    public int completed_requests { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
