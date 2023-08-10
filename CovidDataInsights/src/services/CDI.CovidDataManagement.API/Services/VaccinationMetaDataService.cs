using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IVaccinationMetaDataService
    {
        Task IntegrateVaccinationMetaDataAsync();
    }
    public class VaccinationMetaDataService : IVaccinationMetaDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<VaccinationMetaDataModel> _vaccinationMetaDataReaderService;
        private readonly IVaccinationMetaDataRepository _vaccinationMetaDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        public VaccinationMetaDataService(IOptions<CsvFileSettings> csvFileSettings,
                              ICsvFileReaderService<VaccinationMetaDataModel> vaccinationMetaDataReaderService,
                              IVaccinationMetaDataRepository vaccinationMetaDataRepository,
                              IFileIntegrationRepository integrationRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _vaccinationMetaDataReaderService = vaccinationMetaDataReaderService;
            _vaccinationMetaDataRepository = vaccinationMetaDataRepository;
            _integrationRepository = integrationRepository;
        }

        public async Task IntegrateVaccinationMetaDataAsync()
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
    }
}