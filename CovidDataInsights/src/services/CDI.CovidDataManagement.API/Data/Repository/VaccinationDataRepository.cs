#nullable disable
using CDI.CovidDataManagement.API.DTO;
using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data.Repository
{
    public interface IVaccinationDataRepository
    {
        Task AddVaccinationDataRangeAsync(IEnumerable<VaccinationDataModel> vaccinationDataList);
        Task<List<VaccinationDataModel>> GetAllAsync();
        Task<List<VaccinationDataDto>> GetAllVaccinationDataByMaxIntegrationIdAsync(string filename);
        Task<VaccinationDataDto> GetVaccinationDataByMaxIntegrationIdAndCountryAsync(string filename, string country = null);
        Task<int> GetTotalCountAsync();
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
        public async Task<List<VaccinationDataDto>> GetAllVaccinationDataByMaxIntegrationIdAsync(string filename)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = from vd in _context.VaccinationData
                        where vd.IntegrationId == maxIntegrationId.Value
                        select new VaccinationDataDto
                        {
                            Region = vd.Country,
                            TotalVaccineDoses = vd.TotalVaccinations,
                            PersonsVaccinatedAtLeastOneDose = vd.PersonsVaccinated_1Plus_Dose,
                            PersonsVaccinatedWithCompletePrimarySeries = vd.PersonsLastDose
                        };

            return await query.ToListAsync();
        }

        public async Task<VaccinationDataDto> GetVaccinationDataByMaxIntegrationIdAndCountryAsync(string filename, string country = null)
        {
            var maxIntegrationId = await GetMaxIntegrationIdAsync(filename);

            var query = _context.VaccinationData
                        .Where(vd => vd.IntegrationId == maxIntegrationId.Value);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(vd => vd.Country == country);
            }

            var vaccinationData = await query
                .Select(vd => new VaccinationDataDto
                {
                    Region = vd.Country,
                    TotalVaccineDoses = vd.TotalVaccinations,
                    PersonsVaccinatedAtLeastOneDose = vd.PersonsVaccinated_1Plus_Dose,
                    PersonsVaccinatedWithCompletePrimarySeries = vd.PersonsLastDose
                })
                .FirstOrDefaultAsync();

            return vaccinationData;
        }
    }
}