using CDI.CovidApp.MVC.Extensions;
using CDI.CovidApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidApp.MVC.Services
{
    public interface IGeoJsonDataService
    {
        Task<List<GeoJsonViewModel>> GetGeoJsonData();
    }
    public class GeoJsonDataService : Service, IGeoJsonDataService
    {
        private readonly HttpClient _httpClient;

        public GeoJsonDataService(HttpClient httpClient,
                                IOptions<AppSettings> appSettings)
        {
            httpClient.BaseAddress = new Uri(appSettings.Value.GeoDataUrl);
            _httpClient = httpClient;
        }

        public async Task<List<GeoJsonViewModel>> GetGeoJsonData()
        {
            var response = await _httpClient.GetAsync("/api/Geospatial/geojson/getall");

            return await DeserializeJsonArrayObjectResponse<GeoJsonViewModel>(response);
        }
    }
}