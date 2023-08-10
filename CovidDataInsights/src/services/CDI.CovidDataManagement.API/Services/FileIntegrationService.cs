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
            var csvFolderPath = _csvFileSettings.CsvPath;
            var vaccinationDataFilename = _csvFileSettings?.VaccinationDataFile;
            var vaccinationDataFile = Path.Combine(csvFolderPath ?? string.Empty, vaccinationDataFilename ?? string.Empty);

            if (File.Exists(vaccinationDataFile))
            {
                var (vaccinationData, numberOfRows) = _vaccinationDataReaderService.ReadCsvFile(vaccinationDataFile);
                var rowsIntegrated = vaccinationData.Count();

                if (vaccinationDataFilename != null)
                {
                    var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(vaccinationDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    foreach (var data in vaccinationData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    await _vaccinationDataRepository.AddVaccinationDataRangeAsync(vaccinationData);
                }
            }
        }

        private async Task IntegrateVaccinationMetaDataAsync()
        {
            var csvFolderPath = _csvFileSettings.CsvPath;
            var vaccinationMetaDataFilename = _csvFileSettings?.VaccinationMetadataFile;
            var vaccinationMetaDataFile = Path.Combine(csvFolderPath ?? string.Empty, vaccinationMetaDataFilename ?? string.Empty);

            if (File.Exists(vaccinationMetaDataFile))
            {
                var (vaccinationMetaData, numberOfRows) = _vaccinationMetaDataReaderService.ReadCsvFile(vaccinationMetaDataFile);
                var rowsIntegrated = vaccinationMetaData.Count();

                if (vaccinationMetaDataFilename != null)
                {
                    var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(vaccinationMetaDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    foreach (var data in vaccinationMetaData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    await _vaccinationMetaDataRepository.AddVaccinationMetaDataRangeAsync(vaccinationMetaData);
                }
            }
        }

        private async Task IntegrateWhoGlobalDataAsync()
        {
            var csvFolderPath = _csvFileSettings.CsvPath;
            var whoGlobalDataFilename = _csvFileSettings?.GlobalDataFile;
            var whoGlobalDataFile = Path.Combine(csvFolderPath ?? string.Empty, whoGlobalDataFilename ?? string.Empty);

            if (File.Exists(whoGlobalDataFile))
            {
                var (whoGlobalData, numberOfRows) = _whoGlobalDataReaderService.ReadCsvFile(whoGlobalDataFile);
                var rowsIntegrated = whoGlobalData.Count();

                if (whoGlobalDataFilename != null)
                {
                    var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(whoGlobalDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    foreach (var data in whoGlobalData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    await _whoGlobalDataRepository.AddWhoGlobalDataRangeAsync(whoGlobalData);
                }
            }
        }

        private async Task IntegrateWhoGlobalTableDataAsync()
        {
            var csvFolderPath = _csvFileSettings.CsvPath;
            var whoGlobalTableDataFilename = _csvFileSettings?.GlobalTableDataFile;
            var whoGlobalTableDataFile = Path.Combine(csvFolderPath ?? string.Empty, whoGlobalTableDataFilename ?? string.Empty);

            if (File.Exists(whoGlobalTableDataFile))
            {
                var (whoGlobalTableData, numberOfRows) = _whoGlobalTableDataReaderService.ReadCsvFile(whoGlobalTableDataFile);
                var rowsIntegrated = whoGlobalTableData.Count();

                if (whoGlobalTableDataFilename != null)
                {
                    var integrationModel = IntegrationRecordFactory.CreateIntegrationRecord(whoGlobalTableDataFilename, numberOfRows, rowsIntegrated);
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
}