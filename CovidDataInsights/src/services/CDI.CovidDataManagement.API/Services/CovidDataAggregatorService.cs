using CDI.Core.DomainObjects;
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
        private readonly CountryNameMapper _countryNameMapper;

        public CovidDataAggregatorService(IWhoGlobalTableDataService whoGlobalTableDataService,
                                          IVaccinationDataService vaccinationDataService,
                                          IVaccinationMetaDataService vaccinationMetaDataService,
                                          IWhoGlobalDataService whoGlobalDataService,
                                          IConfiguration configuration)
        {
            _whoGlobalTableDataService = whoGlobalTableDataService;
            _vaccinationDataService = vaccinationDataService;
            _vaccinationMetaDataService = vaccinationMetaDataService;
            _whoGlobalDataService = whoGlobalDataService;
            var mappingFilePath = configuration["JsonFiles:CountryNameMappingFile"];

            if (string.IsNullOrEmpty(mappingFilePath))
            {
                throw new InvalidOperationException("CountryNameMappingFilePath is missing in configuration.");
            }

            _countryNameMapper = new CountryNameMapper(mappingFilePath);
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
            var vaccinationData = await _vaccinationDataService.GetAllVaccinationDataAsync();
            var casesAndDeathsData = await _whoGlobalTableDataService.GetAllCasesAndDeathsDataAsync();

            var covidData = MergeCovidData(vaccinationData, casesAndDeathsData);

            return covidData;
        }

        public async Task<CovidDataDto> GetCovidDataByCountryAsync(string? country = null)
        {
            string region = string.IsNullOrEmpty(country) ? "Global" : country;

            if (string.IsNullOrEmpty(country))
            {
                var globalVaccinationData = await _vaccinationDataService.GetAllVaccinationDataAsync();
                var globalCasesAndDeathsData = await _whoGlobalTableDataService.GetAllCasesAndDeathsDataAsync();

                var globalCovidData = MergeCovidData(globalVaccinationData, globalCasesAndDeathsData, region);

                return globalCovidData.Single();
            }

            var vaccinationData = await _vaccinationDataService.GetVaccinationDataByCountryAsync(country);
            var casesAndDeathsData = await _whoGlobalTableDataService.GetTotalCasesAndDeathsDataByCountryAsync(country);

            var covidData = GenerateCovidDataDto(vaccinationData, casesAndDeathsData);

            return covidData;
        }

        private IEnumerable<CovidDataDto> MergeCovidData(IEnumerable<VaccinationDataDto> vaccinationData,
                                                         IEnumerable<CasesAndDeathsDataDto> casesAndDeathsData,
                                                         string? country = null)
        {
            if (string.IsNullOrEmpty(country))
            {
                // Merge the data using "Region"
                return from vcData in vaccinationData
                       join cdData in casesAndDeathsData on _countryNameMapper.MapCountryNameByValue(vcData.Region) equals _countryNameMapper.MapCountryNameByValue(cdData.Region)
                       select new CovidDataDto
                       {
                           Region = vcData.Region,
                           TotalVaccineDoses = vcData.TotalVaccineDoses,
                           TotalPersonsVaccinatedAtLeastOneDose = vcData.PersonsVaccinatedAtLeastOneDose,
                           TotalPersonsVaccinatedWithCompletePrimarySeries = vcData.PersonsVaccinatedWithCompletePrimarySeries,
                           NewCasesLast7Days = cdData.NewCasesLast7Days,
                           CumulativeCases = cdData.CumulativeCases,
                           NewDeathsLast7Days = cdData.NewDeathsLast7Days,
                           CumulativeDeaths = cdData.CumulativeDeaths
                       };
            }
            else
            {
                var globalCovidData = new CovidDataDto
                {
                    Region = country,
                    TotalVaccineDoses = vaccinationData.Sum(vcData => vcData.TotalVaccineDoses),
                    TotalPersonsVaccinatedAtLeastOneDose = vaccinationData.Sum(vcData => vcData.PersonsVaccinatedAtLeastOneDose),
                    TotalPersonsVaccinatedWithCompletePrimarySeries = vaccinationData.Sum(vcData => vcData.PersonsVaccinatedWithCompletePrimarySeries),
                    NewCasesLast7Days = casesAndDeathsData.Sum(cdData => cdData.NewCasesLast7Days),
                    CumulativeCases = casesAndDeathsData.Sum(cdData => cdData.CumulativeCases),
                    NewDeathsLast7Days = casesAndDeathsData.Sum(cdData => cdData.NewDeathsLast7Days),
                    CumulativeDeaths = casesAndDeathsData.Sum(cdData => cdData.CumulativeDeaths)
                };

                return new List<CovidDataDto> { globalCovidData };
            }
        }
        private static CovidDataDto GenerateCovidDataDto(VaccinationDataDto vaccinationData, CasesAndDeathsDataDto casesAndDeathsData)
        {
            return new CovidDataDto
            {
                Region = vaccinationData.Region,
                TotalVaccineDoses = vaccinationData.TotalVaccineDoses,
                TotalPersonsVaccinatedAtLeastOneDose = vaccinationData.PersonsVaccinatedAtLeastOneDose,
                TotalPersonsVaccinatedWithCompletePrimarySeries = vaccinationData.PersonsVaccinatedWithCompletePrimarySeries,
                NewCasesLast7Days = casesAndDeathsData.NewCasesLast7Days,
                CumulativeCases = casesAndDeathsData.CumulativeCases,
                NewDeathsLast7Days = casesAndDeathsData.NewDeathsLast7Days,
                CumulativeDeaths = casesAndDeathsData.CumulativeDeaths
            };
        }
    }
}