using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalTableDataRepository
    {
        Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList);
        Task<int> GetTotalCountAsync();
    }
    public class WhoGlobalTableDataRepository : IWhoGlobalTableDataRepository
    {
        private readonly ApplicationDbContext _context;

        public WhoGlobalTableDataRepository(ApplicationDbContext context)
        {
            _context = context;
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

        private async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }
    }
}
