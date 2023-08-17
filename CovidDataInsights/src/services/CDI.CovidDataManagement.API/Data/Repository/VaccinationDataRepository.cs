using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationDataRepository
    {
        Task<List<VaccinationDataModel>> GetAllAsync();
        Task<int> GetTotalCountAsync();
        Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList);
    }
    public class VaccinationDataRepository : Repository<VaccinationDataModel>, IVaccinationDataRepository
    {
        public VaccinationDataRepository(ApplicationDbContext context) : base(context)
        {   }

        public async Task<List<VaccinationDataModel>> GetAllAsync()
        {
            return await (_context.VaccinationData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<VaccinationDataModel>()));
        }

        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.VaccinationData != null ? await _context.VaccinationData.CountAsync() : 0;

            return totalCount;
        }

        public async Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList)
        {
            await (_context.VaccinationData?.AddRangeAsync(vaccinationDataList) ?? Task.CompletedTask);
            await PersistData();
        }
    }
}