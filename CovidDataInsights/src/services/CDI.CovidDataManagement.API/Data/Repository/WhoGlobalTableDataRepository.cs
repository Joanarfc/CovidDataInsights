using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalTableDataRepository
    {
        Task<List<WhoGlobalTableDataModel>> GetAllAsync();
        Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList);
        Task<int> GetTotalCountAsync();
    }
    public class WhoGlobalTableDataRepository : Repository<WhoGlobalTableDataModel>, IWhoGlobalTableDataRepository
    {
        public WhoGlobalTableDataRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task<List<WhoGlobalTableDataModel>> GetAllAsync()
        {
            return await(_context.WhoGlobalTableData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<WhoGlobalTableDataModel>()));
        }
        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.WhoGlobalTableData != null ? await _context.WhoGlobalTableData.CountAsync() : 0;

            return totalCount;
        }

        public async Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList)
        {
            await (_context.WhoGlobalTableData?.AddRangeAsync(whoGlobalTableDataList) ?? Task.CompletedTask);
            await PersistData();
        }
    }
}