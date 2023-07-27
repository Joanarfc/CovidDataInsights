using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IFileIntegrationRepository
    {
        Task<IEnumerable<IntegrationModel>> GetAll();
        Task AddAsync(IntegrationModel model);
    }
    public class FileIntegrationRepository : IFileIntegrationRepository
    {
        private readonly ApplicationDbContext _context;

        public FileIntegrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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

        private async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}