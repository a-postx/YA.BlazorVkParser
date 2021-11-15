using YA.WebClient.Application.Models.Dto;

namespace YA.WebClient.Application.Interfaces;

public interface IRuntimeGeoDataService
{
    Task<GeoInfo> GetGeoInfoAsync(CancellationToken cancellationToken);
}
