using CDI.CovidDataManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidDataManagement.API.Controllers
{
    [ApiController]
    [Route("api/covid-data")]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataAggregatorService _covidDataAggregatorService;

        public CovidDataController(ICovidDataAggregatorService covidDataAggregatorService)
        {
            _covidDataAggregatorService = covidDataAggregatorService;
        }

        [HttpPost("integration")]
        public async Task<IActionResult> IntegrateCovidCsvData()
        {
            await _covidDataAggregatorService.IntegrateCovidCsvDataAsync();

            return Ok();
        }

        [HttpGet("by-country")]
        public async Task<ActionResult<long?>> GetTotalCovidDataByCountry([FromQuery] string? country = null)
        {
            var totalVaccineDoses = await _covidDataAggregatorService.GetCovidDataByCountryAsync(country);
            return Ok(totalVaccineDoses);
        }
        [HttpGet("all")]
        public async Task<ActionResult<long?>> GetAllCovidData()
        {
            var totalVaccineDoses = await _covidDataAggregatorService.GetAllCovidDataAsync();
            return Ok(totalVaccineDoses);
        }
    }
}