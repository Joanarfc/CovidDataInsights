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

        [HttpPost("covid-data-integration")]
        public async Task<IActionResult> IntegrateCovidCsvData()
        {
            await _covidDataAggregatorService.IntegrateCovidCsvDataAsync();

            return Ok();
        }

        [HttpGet("total-covid-data")]
        public async Task<ActionResult<long?>> GetTotalCovidData([FromQuery] string? country = null)
        {
            var totalVaccineDoses = await _covidDataAggregatorService.GetCovidDataAsync(country);
            return Ok(totalVaccineDoses);
        }
    }
}