using CDI.GeoSpatialDataLoader.API.Data.Repository;
using CDI.GeoSpatialDataLoader.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CDI.GeoSpatialDataLoader.API.Controllers
{
    [ApiController]
    [Route("[controller")]
    public class GeoSpatialController : ControllerBase
    {
        private readonly IGeoSpatialRepository _geoSpatialRepository;

        public GeoSpatialController(IGeoSpatialRepository geoSpatialRepository)
        {
            _geoSpatialRepository = geoSpatialRepository;
        }
        [HttpGet("/geojson/getall")]
        public async Task<ActionResult<IEnumerable<GeoSpatialModel>>> GetAll()
        {
            var geoJsonData = await _geoSpatialRepository.GetAll();
            return Ok(geoJsonData);
        }
    }
}