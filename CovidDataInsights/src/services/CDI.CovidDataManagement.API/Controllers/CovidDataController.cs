using CDI.CovidDataManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidDataManagement.API.Controllers
{
    [ApiController]
    [Route("api/covid-data")]
    public class CovidDataController : ControllerBase
    {
        private readonly IVaccinationDataService _vaccinationDataService;
        private readonly IVaccinationMetaDataService _vaccinationMetaDataService;
        private readonly IWhoGlobalDataService _whoGlobalDataService;
        private readonly IWhoGlobalTableDataService _whoGlobalTableDataService;
        private readonly ICovidDataAggregatorService _covidDataAggregatorService;

        public CovidDataController(IVaccinationDataService vaccinationDataService,
                                 IVaccinationMetaDataService vaccinationMetaDataService,
                                 IWhoGlobalDataService whoGlobalDataService,
                                 IWhoGlobalTableDataService whoGlobalTableDataService,
                                 ICovidDataAggregatorService covidDataAggregatorService)
        {
            _vaccinationDataService = vaccinationDataService;
            _vaccinationMetaDataService = vaccinationMetaDataService;
            _whoGlobalDataService = whoGlobalDataService;
            _whoGlobalTableDataService = whoGlobalTableDataService;
            _covidDataAggregatorService = covidDataAggregatorService;
        }

        [HttpPost("covid-data-integration")]
        public async Task<IActionResult> IntegrateCovidCsvData()
        {
                await _vaccinationDataService.IntegrateVaccinationDataAsync();
                await _vaccinationMetaDataService.IntegrateVaccinationMetaDataAsync();
                await _whoGlobalDataService.IntegrateWhoGlobalDataAsync();
                await _whoGlobalTableDataService.IntegrateWhoGlobalTableDataAsync();

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