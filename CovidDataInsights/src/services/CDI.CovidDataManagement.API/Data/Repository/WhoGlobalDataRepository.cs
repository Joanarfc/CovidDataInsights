using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalDataRepository
    {
        Task AddWhoGlobalDataRangeAsync(IEnumerable<WhoGlobalDataModel> whoGlobalDataList);
        Task<int> GetTotalCountAsync();
    }
    public class WhoGlobalDataRepository : IWhoGlobalDataRepository
    {
        private readonly ApplicationDbContext _context;

        public WhoGlobalDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.WhoGlobalData != null ? await _context.WhoGlobalData.CountAsync() : 0;

            return totalCount;
        }

        public async Task AddWhoGlobalDataRangeAsync(IEnumerable<WhoGlobalDataModel> whoGlobalDataList)
        {
            await (_context.WhoGlobalData?.AddRangeAsync(whoGlobalDataList) ?? Task.CompletedTask);
            await PersistData();
        }

        private async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }
    }
}
