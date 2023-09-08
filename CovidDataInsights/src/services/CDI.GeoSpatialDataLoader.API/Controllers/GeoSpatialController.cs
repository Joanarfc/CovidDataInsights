using CDI.GeoSpatialDataLoader.API.Models;
using CDI.GeoSpatialDataLoader.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDI.GeoSpatialDataLoader.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoSpatialController : ControllerBase
    {
        private readonly ILogger<GeoSpatialController> _logger;
        private readonly IGeoSpatialService _geoSpatialService;

        public GeoSpatialController(ILogger<GeoSpatialController> logger,
                                    IGeoSpatialService geoSpatialService)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into {nameof(IndicatorService)}", nameof(GeoSpatialController));
            _geoSpatialService = geoSpatialService;
        }

        [HttpPost("load-data")]
        public async Task<IActionResult> LoadDataAsync()
        {
            _logger.LogInformation("LoadData action started");

            // Call the service method to load and save data
            var geoJsonData = await _geoSpatialService.LoadAndSaveDataAsync();

            if (geoJsonData != null)
            {
                _logger.LogInformation("Data loading and saving completed successfully.");
                return Ok(geoJsonData);
            }
            else
            {
                _logger.LogInformation("Data loading and saving skipped.");
                return NoContent();
            }
        }

        [HttpGet("geojson/getall")]
        public async Task<ActionResult<IEnumerable<GeoSpatialModel>>> GetAllAsync()
        {
            _logger.LogInformation("GetAll action started");

            var geoJsonData = await _geoSpatialService.GetAllAsync();

            if (geoJsonData != null)
            {
                _logger.LogInformation("GeoSpatial data retrieved successfully");
                return Ok(geoJsonData);
            }
            else
            {
                _logger.LogInformation("No GeoSpatial data found.");
                return NotFound();
            }
        }
    }
}