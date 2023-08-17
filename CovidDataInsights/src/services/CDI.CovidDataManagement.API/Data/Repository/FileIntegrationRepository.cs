using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IFileIntegrationRepository
    {
        Task<IEnumerable<IntegrationModel>> GetAll();
        Task AddAsync(IntegrationModel model);
    }
    public class FileIntegrationRepository : Repository<IntegrationModel>, IFileIntegrationRepository
    {
        public FileIntegrationRepository(ApplicationDbContext context) : base(context)
        {   }

        public async Task<IEnumerable<IntegrationModel>> GetAll()
        {
            // Retrieve all integration data from the repository
            return _context.IntegrationData == null ? Enumerable.Empty<IntegrationModel>() : await _context.IntegrationData.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(IntegrationModel model)
        {
            _context.IntegrationData?.Add(model);
            await PersistData();
        }
    }
}