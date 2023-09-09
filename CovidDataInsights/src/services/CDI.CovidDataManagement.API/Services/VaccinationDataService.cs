using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.DTO;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Factories;
using CDI.CovidDataManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IVaccinationDataService
    {
        Task IntegrateVaccinationDataAsync();
        Task<CovidDataDto> GetTotalVaccinationDataAsync(string? country = null);
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
        public async Task<CovidDataDto> GetTotalVaccinationDataAsync(string? country = null)
        {
            var (_, csvFilename) = ExtractFilename();

            if (string.IsNullOrEmpty(csvFilename))
            {
                throw new InvalidOperationException("CSV filename is missing.");
            }

            var totalVaccineDoses = await _vaccinationDataRepository.GetTotalVaccineDosesAsync(csvFilename, country);
            var totalPersonsVaccinatedAtLeastOneDose = await _vaccinationDataRepository.GetTotalPersonsVaccinatedAtLeastOneDoseAsync(csvFilename, country);
            var totalPersonsVaccinatedWithCompletePrimarySeries = await _vaccinationDataRepository.GetTotalPersonsVaccinatedWithCompletePrimarySeriesAsync(csvFilename, country);

            var region = string.IsNullOrWhiteSpace(country) ? "Global" : country;

            return new CovidDataDto
            {
                Region = region,
                TotalVaccineDoses = totalVaccineDoses,
                TotalPersonsVaccinatedAtLeastOneDose = totalPersonsVaccinatedAtLeastOneDose,
                TotalPersonsVaccinatedWithCompletePrimarySeries = totalPersonsVaccinatedWithCompletePrimarySeries
            };
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