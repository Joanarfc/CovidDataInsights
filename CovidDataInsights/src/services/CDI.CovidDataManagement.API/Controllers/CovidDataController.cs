﻿using CDI.CovidDataManagement.API.Services;
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

        public CovidDataController(IVaccinationDataService vaccinationDataService,
                                 IVaccinationMetaDataService vaccinationMetaDataService,
                                 IWhoGlobalDataService whoGlobalDataService,
                                 IWhoGlobalTableDataService whoGlobalTableDataService)
        {
            _vaccinationDataService = vaccinationDataService;
            _vaccinationMetaDataService = vaccinationMetaDataService;
            _whoGlobalDataService = whoGlobalDataService;
            _whoGlobalTableDataService = whoGlobalTableDataService;
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

        [HttpGet("total-vaccination-data")]
        public async Task<ActionResult<long?>> GetTotalVaccinationData([FromQuery] string? country = null)
        {
            var totalVaccineDoses = await _vaccinationDataService.GetTotalVaccinationDataAsync(country);
            return Ok(totalVaccineDoses);
        }
    }
}