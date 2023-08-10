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
    }
}