using CDI.CovidApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidApp.MVC.Controllers
{
    public class CovidDataController : Controller
    {
        private readonly ICovidWithGeoJsonDataService _covidWithGeoJsonDataService;

        public CovidDataController(ICovidWithGeoJsonDataService covidWithGeoJsonDataService)
        {
            _covidWithGeoJsonDataService = covidWithGeoJsonDataService;
        }
        [HttpGet]
        [Route("covid-data/by-country")]
        public async Task<IActionResult> Index([FromQuery] string? country = null)
        {
            var covidData = await _covidWithGeoJsonDataService.GetCovidWithGeoJsonByCountryDataAsync(country);
            ViewBag.Country = country;

            return Ok(covidData);
        }
        [HttpGet]
        [Route("covid-geojson-data")]
        public async Task<IActionResult> GetCovidGeojsonDataAsync()
        {
            var allCovidData = await _covidWithGeoJsonDataService.GetCovidWithGeoJsonDataAsync();

            return Ok(allCovidData);
        }
    }
}