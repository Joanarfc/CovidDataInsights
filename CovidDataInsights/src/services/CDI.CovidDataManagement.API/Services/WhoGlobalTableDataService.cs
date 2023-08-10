using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IWhoGlobalTableDataService
    {
        Task IntegrateWhoGlobalTableDataAsync();
    }
    public class WhoGlobalTableDataService : IWhoGlobalTableDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<WhoGlobalTableDataModel> _whoGlobalTableDataReaderService;
        private readonly IWhoGlobalTableDataRepository _whoGlobalTableDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        public WhoGlobalTableDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<WhoGlobalTableDataModel> whoGlobalTableDataReaderService,
                                      IWhoGlobalTableDataRepository whoGlobalTableDataRepository,
                                      IFileIntegrationRepository integrationRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _whoGlobalTableDataReaderService = whoGlobalTableDataReaderService;
            _whoGlobalTableDataRepository = whoGlobalTableDataRepository;
            _integrationRepository = integrationRepository;
        }

        public async Task IntegrateWhoGlobalTableDataAsync()
        {
            var csvUrl = _csvFileSettings?.GlobalTableDataFile;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                var csvFilename = Path.GetFileName(csvUrl);
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
        }
    }
}