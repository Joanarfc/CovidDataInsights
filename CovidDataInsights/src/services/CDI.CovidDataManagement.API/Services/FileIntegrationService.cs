using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;
using CDI.CovidDataManagement.API.Factories;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IFileIntegrationService
    {
        Task IntegrateCsvDataAsync();
    }
    public class FileIntegrationService : IFileIntegrationService
    {
        // Dependencies
        private readonly ICsvFileReaderService<VaccinationDataModel> _vaccinationDataReaderService;
        private readonly ICsvFileReaderService<VaccinationMetaDataModel> _vaccinationMetaDataReaderService;
        private readonly ICsvFileReaderService<WhoGlobalDataModel> _whoGlobalDataReaderService;
        private readonly ICsvFileReaderService<WhoGlobalTableDataModel> _whoGlobalTableDataReaderService;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly IVaccinationDataRepository _vaccinationDataRepository;
        private readonly IVaccinationMetaDataRepository _vaccinationMetaDataRepository;
        private readonly IWhoGlobalDataRepository _whoGlobalDataRepository;
        private readonly IWhoGlobalTableDataRepository _whoGlobalTableDataRepository;
        private readonly CsvFileSettings _csvFileSettings;

        public FileIntegrationService(IFileIntegrationRepository integrationRepository,
                                     IOptions<CsvFileSettings> csvFileSettings,
                                     IVaccinationDataRepository vaccinationDataRepository,
                                     IVaccinationMetaDataRepository vaccinationMetaDataRepository,
                                     IWhoGlobalDataRepository whoGlobalDataRepository,
                                     IWhoGlobalTableDataRepository whoGlobalTableDataRepository,
                                     ICsvFileReaderService<VaccinationDataModel> vaccinationDataReaderService,
                                     ICsvFileReaderService<VaccinationMetaDataModel> vaccinationMetaDataReaderService,
                                     ICsvFileReaderService<WhoGlobalDataModel> whoGlobalDataReaderService,
                                     ICsvFileReaderService<WhoGlobalTableDataModel> whoGlobalTableDataReaderService)
        {
            _csvFileSettings = csvFileSettings.Value;
            _integrationRepository = integrationRepository;
            _vaccinationDataRepository = vaccinationDataRepository;
            _vaccinationMetaDataRepository = vaccinationMetaDataRepository;
            _whoGlobalDataRepository = whoGlobalDataRepository;
            _whoGlobalTableDataRepository = whoGlobalTableDataRepository;
            _vaccinationDataReaderService = vaccinationDataReaderService;
            _vaccinationMetaDataReaderService = vaccinationMetaDataReaderService;
            _whoGlobalDataReaderService = whoGlobalDataReaderService;
            _whoGlobalTableDataReaderService = whoGlobalTableDataReaderService;
        }

        public async Task IntegrateCsvDataAsync()
        {
            await IntegrateVaccinationDataAsync();
            await IntegrateVaccinationMetaDataAsync();
            await IntegrateWhoGlobalDataAsync();
            await IntegrateWhoGlobalTableDataAsync();
        }

        private async Task IntegrateVaccinationDataAsync()
        {
            var csvUrl = _csvFileSettings?.VaccinationDataFile;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                var csvFilename = Path.GetFileName(csvUrl);
                var (vaccinationData, numberOfRows) = await _vaccinationDataReaderService.ReadCsvFile(csvUrl);
                var rowsIntegrated = vaccinationData.Count();

                var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(csvFilename, numberOfRows, rowsIntegrated);
                await _integrationRepository.AddAsync(integrationModel);

                foreach (var data in vaccinationData)
                {
                    data.IntegrationId = integrationModel.Id;
                }

                await _vaccinationDataRepository.AddVaccinationDataRangeAsync(vaccinationData);
            }
        }

        private async Task IntegrateVaccinationMetaDataAsync()
        {
            var csvUrl = _csvFileSettings?.VaccinationMetadataFile;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                var csvFilename = Path.GetFileName(csvUrl);
                var (vaccinationMetaData, numberOfRows) = await _vaccinationMetaDataReaderService.ReadCsvFile(csvUrl);
                var rowsIntegrated = vaccinationMetaData.Count();

                var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(csvFilename, numberOfRows, rowsIntegrated);
                await _integrationRepository.AddAsync(integrationModel);

                foreach (var data in vaccinationMetaData)
                {
                    data.IntegrationId = integrationModel.Id;
                }

                await _vaccinationMetaDataRepository.AddVaccinationMetaDataRangeAsync(vaccinationMetaData);
            }
        }

        private async Task IntegrateWhoGlobalDataAsync()
        {
            var csvUrl = _csvFileSettings?.GlobalDataFile;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                var csvFilename = Path.GetFileName(csvUrl);
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
        }

        private async Task IntegrateWhoGlobalTableDataAsync()
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