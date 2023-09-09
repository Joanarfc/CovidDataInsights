#nullable disable
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public class Repository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
        protected async Task<Guid?> GetMaxIntegrationIdAsync(string filename)
        {
            return await _context.IntegrationData
                .Where(id => id.FileName == filename)
                .GroupBy(id => id.Id)
                .Select(g => new
                {
                    IntegrationId = g.Key,
                    MaxTimestamp = g.Max(id => id.IntegrationTimestamp)
                })
                .OrderByDescending(g => g.MaxTimestamp)
                .Select(g => g.IntegrationId)
                .FirstOrDefaultAsync();
        }
    }
}