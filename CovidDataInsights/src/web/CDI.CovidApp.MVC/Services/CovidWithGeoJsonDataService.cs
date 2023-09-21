using CDI.CovidApp.MVC.Models;

namespace CDI.CovidApp.MVC.Services
{
    public interface ICovidWithGeoJsonDataService
    {
        Task<List<CovidWithGeoJsonDataViewModel>> GetCovidWithGeoJsonDataAsync();
    }
    public class CovidWithGeoJsonDataService : ICovidWithGeoJsonDataService
    {
        private readonly ICovidDataService _covidDataService;
        private readonly IGeoJsonDataService _geoJsonDataService;

        public CovidWithGeoJsonDataService(ICovidDataService covidDataService, IGeoJsonDataService geoJsonDataService)
        {
            _covidDataService = covidDataService;
            _geoJsonDataService = geoJsonDataService;
        }

        public async Task<List<CovidWithGeoJsonDataViewModel>> GetCovidWithGeoJsonDataAsync()
        {
            var geoJsonDataResponse = await _geoJsonDataService.GetGeoJsonData();
            var covidDataResponse = await _covidDataService.GetAllTotalCovidDataAsync();

            if (covidDataResponse == null || geoJsonDataResponse == null)
            {
                return new List<CovidWithGeoJsonDataViewModel>();
            }

            // Lookup dictionary based on the key (Region) for COVID-19 data
            var covidDataLookup = covidDataResponse.ToLookup(data => data.Region);

            // Combine the data by matching the GeoJSON data with COVID-19 data
            var covidWithGeoJsonData = geoJsonDataResponse
                .Select(geoJson =>
                {
                    // NameEN property is the key used for matching GeoJSON data
                    var nameEN = geoJson.NameEN;

                    // Check if there is COVID-19 data for the same region as NameEN
                    if (covidDataLookup.Contains(nameEN))
                    {
                        var covidDataList = covidDataLookup[nameEN].ToList();

                        // If there are multiple entries with the same name, we will choose the first one
                        var covidData = covidDataList.First();

                        return new CovidWithGeoJsonDataViewModel
                        {
                            CovidData = covidData,
                            GeoJsonData = geoJson
                        };
                    }
                    else
                    {
                        // No matching COVID-19 data for this country
                        return null;
                    }
                })
                .Where(data => data != null)
                .ToList();

            return covidWithGeoJsonData;
        }
    }
}