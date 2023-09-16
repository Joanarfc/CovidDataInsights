using CDI.CovidApp.MVC.Extensions;
using CDI.CovidApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidApp.MVC.Services
{
    public interface ICovidDataService
    {
        Task<CovidDataViewModel> GetTotalCovidDataByCountryAsync(string? country = null);
        Task<List<CovidDataViewModel>?> GetAllTotalCovidDataAsync();
    }
    public class CovidDataService : Service, ICovidDataService
    {
        private readonly HttpClient _httpClient;

        public CovidDataService(HttpClient httpClient,
                                IOptions<AppSettings> appSettings)
        {
            httpClient.BaseAddress = new Uri(appSettings.Value.CovidDataUrl);
            _httpClient = httpClient;
        }

        public async Task<CovidDataViewModel> GetTotalCovidDataByCountryAsync(string? country = null)
        {
            var response = await _httpClient.GetAsync($"/api/covid-data/by-country?country={country}");

            return await DeserializeJsonSingleObjectResponse<CovidDataViewModel>(response);
        }
        public async Task<List<CovidDataViewModel>?> GetAllTotalCovidDataAsync()
        {
            var response = await _httpClient.GetAsync($"/api/covid-data/all");

            return await DeserializeJsonArrayObjectResponse<CovidDataViewModel>(response);
        }
    }
}