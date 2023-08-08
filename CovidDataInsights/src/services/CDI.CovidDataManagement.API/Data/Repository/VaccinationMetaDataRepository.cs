using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationMetaDataRepository
    {
        Task AddVaccinationMetaDataRangeAsync(IEnumerable<VaccinationMetaDataModel> vaccinationMetaDataList);
        Task<int> GetTotalCountAsync();
    }
    public class VaccinationMetaDataRepository : IVaccinationMetaDataRepository
    {
        private readonly ApplicationDbContext _context;

        public VaccinationMetaDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<VaccinationMetaDataModel>> GetAllAsync()
        {
            return await (_context.VaccinationMetaData?.AsNoTracking().ToListAsync() ?? Task.FromResult(new List<VaccinationMetaDataModel>()));
        }

        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = _context.VaccinationMetaData != null ? await _context.VaccinationMetaData.CountAsync() : 0;

            return totalCount;
        }

        public async Task AddVaccinationMetaDataRangeAsync(IEnumerable<VaccinationMetaDataModel> vaccinationMetaDataList)
        {
            await (_context.VaccinationMetaData?.AddRangeAsync(vaccinationMetaDataList) ?? Task.CompletedTask);
            await PersistData();
        }

        private async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }
    }
}