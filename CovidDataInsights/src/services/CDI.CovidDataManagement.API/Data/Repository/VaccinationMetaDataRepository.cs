using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationMetaDataRepository
    {
        Task<List<VaccinationMetaDataModel>> GetAllAsync();
        Task AddVaccinationMetaDataRangeAsync(IEnumerable<VaccinationMetaDataModel> vaccinationMetaDataList);
        Task<int> GetTotalCountAsync();
    }
    public class VaccinationMetaDataRepository : Repository<VaccinationMetaDataModel>, IVaccinationMetaDataRepository
    {
        public VaccinationMetaDataRepository(ApplicationDbContext context) : base(context)
        {  }

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
    }
}