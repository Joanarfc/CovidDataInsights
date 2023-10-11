using CDI.Core.DomainObjects;
using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.DTO;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IWhoGlobalTableDataService
    {
        Task IntegrateWhoGlobalTableDataAsync();
        Task<IEnumerable<CasesAndDeathsDataDto>> GetAllCasesAndDeathsDataAsync();
        Task<CasesAndDeathsDataDto> GetTotalCasesAndDeathsDataByCountryAsync(string? country = null);
    }
    public class WhoGlobalTableDataService : IWhoGlobalTableDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<WhoGlobalTableDataModel> _whoGlobalTableDataReaderService;
        private readonly IWhoGlobalTableDataRepository _whoGlobalTableDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly CsvFileHelper _csvFileHelper;
        private readonly CountryNameMapper _countryNameMapper;

        public WhoGlobalTableDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<WhoGlobalTableDataModel> whoGlobalTableDataReaderService,
                                      IWhoGlobalTableDataRepository whoGlobalTableDataRepository,
                                      IFileIntegrationRepository integrationRepository,
                                      CsvFileHelper csvFileHelper,
                                      IConfiguration configuration)
        {
            _csvFileSettings = csvFileSettings.Value;
            _whoGlobalTableDataReaderService = whoGlobalTableDataReaderService;
            _whoGlobalTableDataRepository = whoGlobalTableDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
            var mappingFilePath = configuration["JsonFiles:CountryNameMappingFile"];

            if (string.IsNullOrEmpty(mappingFilePath))
            {
                throw new InvalidOperationException("CountryNameMappingFilePath is missing in configuration.");
            }

            _countryNameMapper = new CountryNameMapper(mappingFilePath);
        }

        public async Task IntegrateWhoGlobalTableDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var (whoGlobalTableData, numberOfRows) = await _whoGlobalTableDataReaderService.ReadCsvFile(csvUrl);
            var rowsIntegrated = whoGlobalTableData.Count();

            var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(csvFilename, numberOfRows, rowsIntegrated);
            await _integrationRepository.AddAsync(integrationModel);

            foreach (var data in whoGlobalTableData)
            {
                data.IntegrationId = integrationModel.Id;
            }

            await _whoGlobalTableDataRepository.AddWhoGlobalTableDataRangeAsync(whoGlobalTableData);
        }
        public async Task<IEnumerable<CasesAndDeathsDataDto>> GetAllCasesAndDeathsDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var whoGlobalTableData = await _whoGlobalTableDataRepository.GetAllCasesAndDeathsDataByMaxIntegrationIdAsync(csvFilename);
            return whoGlobalTableData;
        }
        public async Task<CasesAndDeathsDataDto> GetTotalCasesAndDeathsDataByCountryAsync(string? country = null)
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            // Map the input country name using the mapper
            string mappedCountryName = _countryNameMapper.MapCountryNameByValue(country);

            var casesAndDeathsDataCountry = await _whoGlobalTableDataRepository.GetCasesAndDeathsDataByMaxIntegrationIdAndCountryAsync(csvFilename, mappedCountryName);

            return new CasesAndDeathsDataDto
            {
                Region = country,
                NewCasesLast7Days = casesAndDeathsDataCountry.NewCasesLast7Days,
                CumulativeCases = casesAndDeathsDataCountry.CumulativeCases,
                NewDeathsLast7Days = casesAndDeathsDataCountry.NewDeathsLast7Days,
                CumulativeDeaths = casesAndDeathsDataCountry.CumulativeDeaths
            };
        }

        private string GetCsvUrl()
        {
            var csvUrl = _csvFileSettings?.GlobalTableDataFile;

            if (string.IsNullOrEmpty(csvUrl))
            {
                throw new FileNotFoundException("CSV URL is missing.");
            }

            return csvUrl;
        }
    }
}