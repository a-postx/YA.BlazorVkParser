namespace YA.WebClient.Application.Models.Dto;

public class ClientInfo
{
    public NavigatorInfo Navigator { get; set; }
    public UaInfo Ua { get; set; }
    public ScreenInfo Screen { get; set; }
    public GeoInfo Geo { get; set; }
}
