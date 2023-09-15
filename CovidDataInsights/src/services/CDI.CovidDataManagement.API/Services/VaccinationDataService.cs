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
        Task<CovidDataDto> GetVaccinationDataByCountryAsync(string? country = null);
    }
    public class VaccinationDataService : IVaccinationDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<VaccinationDataModel> _vaccinationDataReaderService;
        private readonly IVaccinationDataRepository _vaccinationDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly CsvFileHelper _csvFileHelper;

        public VaccinationDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<VaccinationDataModel> vaccinationDataReaderService,
                                      IVaccinationDataRepository vaccinationDataRepository,
                                      IFileIntegrationRepository integrationRepository,
                                      CsvFileHelper csvFileHelper)
        {
            _csvFileSettings = csvFileSettings.Value;
            _vaccinationDataReaderService = vaccinationDataReaderService;
            _vaccinationDataRepository = vaccinationDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
        }

        public async Task IntegrateVaccinationDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = csvUrl != null ? _csvFileHelper.ExtractFilename(csvUrl) : throw new FileNotFoundException("CSV URL is missing.");

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
        public async Task<CovidDataDto> GetVaccinationDataByCountryAsync(string? country = null)
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var totalVaccineDoses = await _vaccinationDataRepository.GetVaccineDosesByCountryAsync(csvFilename, country);
            var totalPersonsVaccinatedAtLeastOneDose = await _vaccinationDataRepository.GetPersonsVaccinatedAtLeastOneDoseByCountryAsync(csvFilename, country);
            var totalPersonsVaccinatedWithCompletePrimarySeries = await _vaccinationDataRepository.GetPersonsVaccinatedWithCompletePrimarySeriesByCountryAsync(csvFilename, country);

            var region = string.IsNullOrWhiteSpace(country) ? "Global" : country;

            return new CovidDataDto
            {
                Region = region,
                TotalVaccineDoses = totalVaccineDoses,
                TotalPersonsVaccinatedAtLeastOneDose = totalPersonsVaccinatedAtLeastOneDose,
                TotalPersonsVaccinatedWithCompletePrimarySeries = totalPersonsVaccinatedWithCompletePrimarySeries
            };
        }
        private string GetCsvUrl()
        {
            var csvUrl = _csvFileSettings?.VaccinationDataFile;

            if (string.IsNullOrEmpty(csvUrl))
            {
                throw new FileNotFoundException("CSV URL is missing.");
            }

            return csvUrl;
        }
    }
}