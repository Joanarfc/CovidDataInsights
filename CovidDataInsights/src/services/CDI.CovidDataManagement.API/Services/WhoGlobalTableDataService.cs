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
        Task<CovidDataDto> GetTotalCasesDataAsync(string? country = null);
        Task<CovidDataDto> GetTotalDeathsDataAsync(string? country = null);
    }
    public class WhoGlobalTableDataService : IWhoGlobalTableDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<WhoGlobalTableDataModel> _whoGlobalTableDataReaderService;
        private readonly IWhoGlobalTableDataRepository _whoGlobalTableDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly CsvFileHelper _csvFileHelper;

        public WhoGlobalTableDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<WhoGlobalTableDataModel> whoGlobalTableDataReaderService,
                                      IWhoGlobalTableDataRepository whoGlobalTableDataRepository,
                                      IFileIntegrationRepository integrationRepository,
                                      CsvFileHelper csvFileHelper)
        {
            _csvFileSettings = csvFileSettings.Value;
            _whoGlobalTableDataReaderService = whoGlobalTableDataReaderService;
            _whoGlobalTableDataRepository = whoGlobalTableDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
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
        public async Task<CovidDataDto> GetTotalCasesDataAsync(string? country = null)
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var totalNewCasesLast7Days = await _whoGlobalTableDataRepository.GetNewCasesLast7DaysAsync(csvFilename, country);
            var totalCumulativeCases = await _whoGlobalTableDataRepository.GetCumulativeCasesAsync(csvFilename, country);

            var region = string.IsNullOrWhiteSpace(country) ? "Global" : country;

            return new CovidDataDto
            {
                Region = region,
                NewCasesLast7Days = totalNewCasesLast7Days,
                CumulativeCases = totalCumulativeCases
            };
        }
        public async Task<CovidDataDto> GetTotalDeathsDataAsync(string? country = null)
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var totalNewDeathsLast7Days = await _whoGlobalTableDataRepository.GetNewDeathsLast7DaysAsync(csvFilename, country);
            var totalCumulativeDeaths = await _whoGlobalTableDataRepository.GetCumulativeDeathsAsync(csvFilename, country);

            var region = string.IsNullOrWhiteSpace(country) ? "Global" : country;

            return new CovidDataDto
            {
                Region = region,
                NewDeathsLast7Days = totalNewDeathsLast7Days,
                CumulativeDeaths = totalCumulativeDeaths
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