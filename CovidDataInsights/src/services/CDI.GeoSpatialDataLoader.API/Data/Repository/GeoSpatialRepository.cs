using CDI.GeoSpatialDataLoader.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.GeoSpatialDataLoader.API.Data.Repository
{
    public interface IGeoSpatialRepository
    {
        Task<IEnumerable<GeoSpatialModel>> GetAllAsync();
        void AddGeoSpatialData(GeoSpatialModel geoSpatialModel);
        Task<int> SaveChangesAsync();
        int GetCountriesCountFromDB();
    }
    public class GeoSpatialRepository : IGeoSpatialRepository
    {
        private readonly ApplicationDataContext _context;
        private readonly ILogger<GeoSpatialRepository> _logger;


        public GeoSpatialRepository(ApplicationDataContext context,
                                    ILogger<GeoSpatialRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into GeoSpatialRepository");
            _logger.LogDebug(1, "GeoSpatialRepository has been constructed");
        }

        public async Task<IEnumerable<GeoSpatialModel>> GetAllAsync()
        {
            _logger.LogInformation("GetAll method started.");

            if (_context.Countries != null)
            {
                _logger.LogInformation("Returning data from GetAll method.");
                return await _context.Countries.ToListAsync();
            }
            else
            {
                _logger.LogInformation("Returning empty data from GetAll method.");
                return Enumerable.Empty<GeoSpatialModel>();
            }
        }

        public int GetCountriesCountFromDB()
        {
            return _context.Countries?.ToList().Count ?? 0;
        }

        public void AddGeoSpatialData(GeoSpatialModel geoSpatialModel)
        {
            _context?.Countries?.Add(geoSpatialModel);
        }

        public async Task<int> SaveChangesAsync()
        {
            _logger.LogInformation("SaveChanges method started.");

            return await _context.SaveChangesAsync();
        }
    }
}