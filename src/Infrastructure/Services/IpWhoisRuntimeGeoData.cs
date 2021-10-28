using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YA.WebClient.Application.Models.Dto;
using YA.WebClient.Constants;
using YA.WebClient.Infrastructure.Services.GeoDataModels;
using YA.WebClient.Application.Interfaces;

namespace YA.WebClient.Infrastructure.Services
{
    public class IpWhoisRuntimeGeoData : IRuntimeGeoDataService
    {
        public IpWhoisRuntimeGeoData(ILogger<IpWhoisRuntimeGeoData> logger, IHttpClientFactory httpClientFactory)
        {
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        private readonly ILogger<IpWhoisRuntimeGeoData> _log;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _providerUrl = "https://ipwhois.app/";

        public async Task<GeoInfo> GetGeoInfoAsync(CancellationToken cancellationToken)
        {
            IpWhoisGeoData geoData = await GetDataAsync(cancellationToken);

            if (geoData != null)
            {
                GeoInfo geoInfo = new GeoInfo { Country = geoData.country, Region = geoData.region };
                return geoInfo;
            }
            else
            {
                return null;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Обработчик уничтожается вместе с HttpClient")]
        private async Task<IpWhoisGeoData> GetDataAsync(CancellationToken cancellationToken)
        {
            IpWhoisGeoData result = null;

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_providerUrl);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", General.AppHttpUserAgent);
                client.Timeout = TimeSpan.FromSeconds(60);

                HttpResponseMessage response = await client.GetAsync(new Uri("json/?lang=ru&objects=country,region", UriKind.Relative), cancellationToken);
                response.EnsureSuccessStatusCode();

                using (Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken))
                {
                    IpWhoisGeoData data = null;

                    data = await JsonSerializer
                            .DeserializeAsync<IpWhoisGeoData>(responseStream, options: null, cancellationToken);

                    if (data != null)
                    {
                        result = data;
                    }
                    else
                    {
                        _log.LogWarning("No geodata available.");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "No geodata available");
            }

            return result;
        }
    }
}
