using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IVaccinationDataService
    {
        Task IntegrateVaccinationDataAsync();
    }
    public class VaccinationDataService : IVaccinationDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<VaccinationDataModel> _vaccinationDataReaderService;
        private readonly IVaccinationDataRepository _vaccinationDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        public VaccinationDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<VaccinationDataModel> vaccinationDataReaderService,
                                      IVaccinationDataRepository vaccinationDataRepository,
                                      IFileIntegrationRepository integrationRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _vaccinationDataReaderService = vaccinationDataReaderService;
            _vaccinationDataRepository = vaccinationDataRepository;
            _integrationRepository = integrationRepository;
        }

        public async Task IntegrateVaccinationDataAsync()
        {
            var (csvUrl, csvFilename) = ExtractFilename();

            if (!string.IsNullOrEmpty(csvFilename))
            {
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
        private (string CsvUrl, string CsvFileName) ExtractFilename()
        {
            var csvUrl = _csvFileSettings?.VaccinationDataFile;
            string csvFilename;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                csvFilename = Path.GetFileName(csvUrl);
            }
            else
            {
                throw new InvalidOperationException("CSV URL is null or empty.");
            }

            return (csvUrl, csvFilename);
        }
    }
}