#nullable disable
using CDI.CovidDataManagement.API.DTO;
using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IWhoGlobalTableDataRepository
    {
        Task AddWhoGlobalTableDataRangeAsync(IEnumerable<WhoGlobalTableDataModel> whoGlobalTableDataList);
        Task<List<WhoGlobalTableDataModel>> GetAllAsync();
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<CasesAndDeathsDataDto>> GetAllCasesAndDeathsDataByMaxIntegrationIdAsync(string filename);
        Task<CasesAndDeathsDataDto> GetCasesAndDeathsDataByMaxIntegrationIdAndCountryAsync(string filename, string country = null);
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
            return await (_context.WhoGlobalTableData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<WhoGlobalTableDataModel>()));
        }
        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.WhoGlobalTableData != null ? await _context.WhoGlobalTableData.CountAsync() : 0;

            return totalCount;
        }
        public async Task<IEnumerable<CasesAndDeathsDataDto>> GetAllCasesAndDeathsDataByMaxIntegrationIdAsync(string filename)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = from cd in _context.WhoGlobalTableData
                        where cd.IntegrationId == maxIntegrationId.Value && cd.Name != "Global"
                        select new CasesAndDeathsDataDto
                        {
                            Region = cd.Name,
                            NewCasesLast7Days = cd.CasesNewlyReportedInLast7Days,
                            CumulativeCases = cd.CasesCumulativeTotal,
                            NewDeathsLast7Days = cd.DeathsNewlyReportedInLast7Days,
                            CumulativeDeaths = cd.DeathsCumulativeTotal
                        };

            return await query.ToListAsync();
        }

        public async Task<CasesAndDeathsDataDto> GetCasesAndDeathsDataByMaxIntegrationIdAndCountryAsync(string filename, string country = null)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.WhoGlobalTableData
                        .Where(cd => cd.IntegrationId == maxIntegrationId.Value && cd.Name != "Global");

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(cd => cd.Name == country);
            }

            var casesAndDeathsData = await query
                .Select(cd => new CasesAndDeathsDataDto
                {
                    Region = cd.Name,
                    NewCasesLast7Days = cd.CasesNewlyReportedInLast7Days,
                    CumulativeCases = cd.CasesCumulativeTotal,
                    NewDeathsLast7Days = cd.DeathsNewlyReportedInLast7Days,
                    CumulativeDeaths = cd.DeathsCumulativeTotal
                })
                .FirstOrDefaultAsync();

            return casesAndDeathsData;
        }
    }
}