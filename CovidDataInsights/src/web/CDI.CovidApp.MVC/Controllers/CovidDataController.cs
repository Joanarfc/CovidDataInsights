using CDI.CovidApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidApp.MVC.Controllers
{
    public class CovidDataController : Controller
    {
        private readonly ICovidDataService _covidDataService;
        private readonly ICovidWithGeoJsonDataService _covidWithGeoJsonDataService;

        public CovidDataController(ICovidDataService covidDataService, ICovidWithGeoJsonDataService covidWithGeoJsonDataService)
        {
            _covidDataService = covidDataService;
            _covidWithGeoJsonDataService = covidWithGeoJsonDataService;
        }
        [HttpGet]
        [Route("")]
        [Route("covid-data/by-country")]
        public async Task<IActionResult> Index([FromQuery] string? country = null)
        {
            var covidData = await _covidDataService.GetTotalCovidDataByCountryAsync(country);
            ViewBag.Country = country;

            return Ok(covidData);
        }
        [HttpGet]
        [Route("covid-data/all")]
        public async Task<IActionResult> GetAllTotalCovidDataAsync()
        {
            var allCovidData = await _covidDataService.GetAllTotalCovidDataAsync();

            return Ok(allCovidData);
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