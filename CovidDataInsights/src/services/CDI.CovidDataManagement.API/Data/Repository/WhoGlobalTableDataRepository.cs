#nullable disable
using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalTableDataRepository
    {
        Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList);
        Task<List<WhoGlobalTableDataModel>> GetAllAsync();
        Task<int> GetTotalCountAsync();
        Task<int?> GetNewCasesLast7DaysAsync(string filename, string country = null);
        Task<long?> GetCumulativeCasesAsync(string filename, string country);
        Task<int?> GetNewDeathsLast7DaysAsync(string filename, string country = null);
        Task<long?> GetCumulativeDeathsAsync(string filename, string country);
    }
    public class WhoGlobalTableDataRepository : Repository<WhoGlobalTableDataModel>, IWhoGlobalTableDataRepository
    {
        public WhoGlobalTableDataRepository(ApplicationDbContext context) : base(context)
        { }
        public async Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList)
        {
            await (_context.WhoGlobalTableData?.AddRangeAsync(whoGlobalTableDataList) ?? Task.CompletedTask);
            await PersistData();
        }

        public async Task<List<WhoGlobalTableDataModel>> GetAllAsync()
        {
            return await(_context.WhoGlobalTableData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<WhoGlobalTableDataModel>()));
        }
        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.WhoGlobalTableData != null ? await _context.WhoGlobalTableData.CountAsync() : 0;

            return totalCount;
        }
        
        public async Task<int?> GetNewCasesLast7DaysAsync(string filename, string country = null)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.WhoGlobalTableData
                .Where(vd => vd.IntegrationId == maxIntegrationId.Value);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(wtd => wtd.Name == country);
            }
            else
            {
                query = query.Where(wtd => wtd.Name == "Global");
            }

            var newCasesLast7Days = await query.SumAsync(wtd => wtd.CasesNewlyReportedInLast7Days);

            return newCasesLast7Days;
        }

        public async Task<long?> GetCumulativeCasesAsync(string filename, string country)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.WhoGlobalTableData
                .Where(vd => vd.IntegrationId == maxIntegrationId.Value);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(wtd => wtd.Name == country);
            }
            else
            {
                query = query.Where(wtd => wtd.Name == "Global");
            }

            var newCasesLast7Days = await query.SumAsync(wtd => wtd.CasesCumulativeTotal);

            return newCasesLast7Days;
        }
        public async Task<int?> GetNewDeathsLast7DaysAsync(string filename, string country = null)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.WhoGlobalTableData
                .Where(vd => vd.IntegrationId == maxIntegrationId.Value);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(wtd => wtd.Name == country);
            }
            else
            {
                query = query.Where(wtd => wtd.Name == "Global");
            }

            var newCasesLast7Days = await query.SumAsync(wtd => wtd.DeathsNewlyReportedInLast7Days);

            return newCasesLast7Days;
        }
        public async Task<long?> GetCumulativeDeathsAsync(string filename, string country)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.WhoGlobalTableData
                .Where(vd => vd.IntegrationId == maxIntegrationId.Value);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(wtd => wtd.Name == country);
            }
            else
            {
                query = query.Where(wtd => wtd.Name == "Global");
            }

            var newCasesLast7Days = await query.SumAsync(wtd => wtd.DeathsCumulativeTotal);

            return newCasesLast7Days;
        }
    }
}