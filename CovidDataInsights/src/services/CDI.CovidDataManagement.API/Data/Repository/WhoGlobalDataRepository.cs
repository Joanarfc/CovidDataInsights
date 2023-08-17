using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalDataRepository
    {
        Task<List<WhoGlobalDataModel>> GetAllAsync();
        Task<int> GetTotalCountAsync();
        Task AddWhoGlobalDataRangeAsync(IEnumerable<WhoGlobalDataModel> whoGlobalDataList);
    }
    public class WhoGlobalDataRepository : Repository<WhoGlobalDataModel>, IWhoGlobalDataRepository
    {
        public WhoGlobalDataRepository(ApplicationDbContext context) : base(context)
        {  }

        public async Task<List<WhoGlobalDataModel>> GetAllAsync()
        {
            return await(_context.WhoGlobalData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<WhoGlobalDataModel>()));
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
    }
}