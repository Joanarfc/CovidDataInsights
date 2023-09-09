using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IWhoGlobalDataService
    {
        Task IntegrateWhoGlobalDataAsync();
    }
    public class WhoGlobalDataService : IWhoGlobalDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<WhoGlobalDataModel> _whoGlobalDataReaderService;
        private readonly IWhoGlobalDataRepository _whoGlobalDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly CsvFileHelper _csvFileHelper;

        public WhoGlobalDataService(IOptions<CsvFileSettings> csvFileSettings,
                      ICsvFileReaderService<WhoGlobalDataModel> whoGlobalDataReaderService,
                      IWhoGlobalDataRepository whoGlobalDataRepository,
                      IFileIntegrationRepository integrationRepository,
                      CsvFileHelper csvFileHelper)
        {
            _csvFileSettings = csvFileSettings.Value;
            _whoGlobalDataReaderService = whoGlobalDataReaderService;
            _whoGlobalDataRepository = whoGlobalDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
        }
        public async Task IntegrateWhoGlobalDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var (whoGlobalData, numberOfRows) = await _whoGlobalDataReaderService.ReadCsvFile(csvUrl);
            var rowsIntegrated = whoGlobalData.Count();

            var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(csvFilename, numberOfRows, rowsIntegrated);
            await _integrationRepository.AddAsync(integrationModel);

            foreach (var data in whoGlobalData)
            {
                data.IntegrationId = integrationModel.Id;
            }

            await _whoGlobalDataRepository.AddWhoGlobalDataRangeAsync(whoGlobalData);
        }
        private string GetCsvUrl()
        {
            var csvUrl = _csvFileSettings?.GlobalDataFile;

            if (string.IsNullOrEmpty(csvUrl))
            {
                throw new FileNotFoundException("CSV URL is missing.");
            }

            return csvUrl;
        }
    }
}