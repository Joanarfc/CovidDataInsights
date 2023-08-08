using CDI.CovidDataManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.CovidDataManagement.API.Controllers
{
    [ApiController]
    [Route("api/csv-data")]
    public class CsvDataController : ControllerBase
    {
        private readonly IFileIntegrationService _integrationService;
        public CsvDataController(IFileIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpPost("covid-data-integration")]
        public async Task<IActionResult> IntegrateCsvData()
        {
            try
            {
                await _integrationService.IntegrateCsvDataAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while integrating CSV files: {ex.Message}");
            }
        }
    }
}