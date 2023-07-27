using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationDataRepository
    {
        Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList);
        Task<int> GetTotalCountAsync();
    }
    public class VaccinationDataRepository : IVaccinationDataRepository
    {
        private readonly ApplicationDbContext _context;

        public VaccinationDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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