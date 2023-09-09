#nullable disable
using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationDataRepository
    {
        Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList);
        Task<List<VaccinationDataModel>> GetAllAsync();
        Task<int> GetTotalCountAsync();
        Task<long?> GetTotalVaccineDosesAsync(string filename, string country = null);
        Task<long?> GetTotalPersonsVaccinatedAtLeastOneDoseAsync(string filename, string country = null);
        Task<long?> GetTotalPersonsVaccinatedWithCompletePrimarySeriesAsync(string filename, string country = null);
    }
    public class VaccinationDataRepository : Repository<VaccinationDataModel>, IVaccinationDataRepository
    {
        public VaccinationDataRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList)
        {
            await (_context.VaccinationData?.AddRangeAsync(vaccinationDataList) ?? Task.CompletedTask);
            await PersistData();
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

        public async Task<long?> GetTotalVaccineDosesAsync(string filename, string country = null)
        {
            var query = _context.IntegrationData
                .Where(id => id.FileName == filename)
                .SelectMany(id => id.VaccinationData);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(vd => vd.Country == country);
            }

            var totalVaccineDoses = await query.SumAsync(vd => vd.TotalVaccinations);

            return totalVaccineDoses;
        }

        public async Task<long?> GetTotalPersonsVaccinatedAtLeastOneDoseAsync(string filename, string country = null)
        {
            var query = _context.IntegrationData
                .Where(id => id.FileName == filename)
                .SelectMany(id => id.VaccinationData);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(vd => vd.Country == country);
            }

            var totalPersonsVaccinatedAtLeastOneDose = await query.SumAsync(vd => vd.PersonsVaccinated_1Plus_Dose);

            return totalPersonsVaccinatedAtLeastOneDose;
        }
        public async Task<long?> GetTotalPersonsVaccinatedWithCompletePrimarySeriesAsync(string filename, string country = null)
        {
            var query = _context.IntegrationData
                .Where(id => id.FileName == filename)
                .SelectMany(id => id.VaccinationData);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(vd => vd.Country == country);
            }

            var totalPersonsVaccinatedWithCompletePrimarySeries = await query.SumAsync(vd => vd.PersonsLastDose);

            return totalPersonsVaccinatedWithCompletePrimarySeries;
        }
    }
}