using CDI.CovidDataManagement.API.DTO;

namespace CDI.CovidDataManagement.API.Services
{
    public interface ICovidDataAggregatorService
    {
        Task IntegrateCovidCsvDataAsync();
        Task<CovidDataDto> GetCovidDataAsync(string? country = null);
    }
    public class CovidDataAggregatorService : ICovidDataAggregatorService
    {
        private readonly IVaccinationDataService _vaccinationDataService;
        private readonly IVaccinationMetaDataService _vaccinationMetaDataService;
        private readonly IWhoGlobalDataService _whoGlobalDataService;
        private readonly IWhoGlobalTableDataService _whoGlobalTableDataService;

        public CovidDataAggregatorService(IWhoGlobalTableDataService whoGlobalTableDataService,
                                          IVaccinationDataService vaccinationDataService,
                                          IVaccinationMetaDataService vaccinationMetaDataService,
                                          IWhoGlobalDataService whoGlobalDataService)
        {
            _whoGlobalTableDataService = whoGlobalTableDataService;
            _vaccinationDataService = vaccinationDataService;
            _vaccinationMetaDataService = vaccinationMetaDataService;
            _whoGlobalDataService = whoGlobalDataService;
        }
        public async Task IntegrateCovidCsvDataAsync()
        {
            await _vaccinationDataService.IntegrateVaccinationDataAsync();
            await _vaccinationMetaDataService.IntegrateVaccinationMetaDataAsync();
            await _whoGlobalDataService.IntegrateWhoGlobalDataAsync();
            await _whoGlobalTableDataService.IntegrateWhoGlobalTableDataAsync();
        }

        public async Task<CovidDataDto> GetCovidDataAsync(string? country = null)
        {
            var vaccinationData = await _vaccinationDataService.GetTotalVaccinationDataAsync(country);
            var casesData = await _whoGlobalTableDataService.GetTotalCasesDataAsync(country);
            var deathsData = await _whoGlobalTableDataService.GetTotalDeathsDataAsync(country);

            var covidData = new CovidDataDto
            {
                Region = casesData.Region,
                TotalVaccineDoses = vaccinationData.TotalVaccineDoses,
                TotalPersonsVaccinatedAtLeastOneDose = vaccinationData.TotalPersonsVaccinatedAtLeastOneDose,
                TotalPersonsVaccinatedWithCompletePrimarySeries = vaccinationData.TotalPersonsVaccinatedWithCompletePrimarySeries,
                NewCasesLast7Days = casesData.NewCasesLast7Days,
                CumulativeCases = casesData.CumulativeCases,
                NewDeathsLast7Days = deathsData.NewDeathsLast7Days,
                CumulativeDeaths = deathsData.CumulativeDeaths
            };
            return covidData;
        }
    }
}