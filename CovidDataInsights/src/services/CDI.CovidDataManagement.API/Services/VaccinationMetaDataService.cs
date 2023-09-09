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
        private readonly CsvFileHelper _csvFileHelper;

        public VaccinationMetaDataService(IOptions<CsvFileSettings> csvFileSettings,
                              ICsvFileReaderService<VaccinationMetaDataModel> vaccinationMetaDataReaderService,
                              IVaccinationMetaDataRepository vaccinationMetaDataRepository,
                              IFileIntegrationRepository integrationRepository,
                              CsvFileHelper csvFileHelper)
        {
            _csvFileSettings = csvFileSettings.Value;
            _vaccinationMetaDataReaderService = vaccinationMetaDataReaderService;
            _vaccinationMetaDataRepository = vaccinationMetaDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
        }

        public async Task IntegrateVaccinationMetaDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

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
        private string GetCsvUrl()
        {
            var csvUrl = _csvFileSettings?.VaccinationMetadataFile;

            if (string.IsNullOrEmpty(csvUrl))
            {
                throw new FileNotFoundException("CSV URL is missing.");
            }

            return csvUrl;
        }
    }
}