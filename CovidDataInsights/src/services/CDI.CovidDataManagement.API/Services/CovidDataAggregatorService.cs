using CDI.CovidDataManagement.API.DTO;

namespace CDI.CovidDataManagement.API.Services
{
    public interface ICovidDataAggregatorService
    {
        Task IntegrateCovidCsvDataAsync();
        Task<IEnumerable<CovidDataDto>> GetAllCovidDataAsync();
        Task<CovidDataDto> GetCovidDataByCountryAsync(string? country = null);
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
        public async Task<IEnumerable<CovidDataDto>> GetAllCovidDataAsync()
        {
            var countries = await _vaccinationDataService.GetAllCountriesAsync();
            var covidDataList = new List<CovidDataDto>();

            foreach (var country in countries)
            {
                var covidData = await GenerateCovidDataDtoAsync(country);
                covidDataList.Add(covidData);
            }

            return covidDataList;
        }

        public async Task<CovidDataDto> GetCovidDataByCountryAsync(string? country = null)
        {
            return await GenerateCovidDataDtoAsync(country);
        }
        private async Task<CovidDataDto> GenerateCovidDataDtoAsync(string? country)
        {
            var vaccinationData = await _vaccinationDataService.GetVaccinationDataByCountryAsync(country);
            var casesData = await _whoGlobalTableDataService.GetTotalCasesDataByCountryAsync(country);
            var deathsData = await _whoGlobalTableDataService.GetTotalDeathsDataByCountryAsync(country);

            return new CovidDataDto
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
        }
    }
}