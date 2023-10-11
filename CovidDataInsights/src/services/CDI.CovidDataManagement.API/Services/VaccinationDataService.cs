using CDI.Core.DomainObjects;
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
        Task<IEnumerable<VaccinationDataDto>> GetAllVaccinationDataAsync();
        Task<VaccinationDataDto> GetVaccinationDataByCountryAsync(string? country = null);
    }
    public class VaccinationDataService : IVaccinationDataService
    {
        private readonly CsvFileSettings _csvFileSettings;
        private readonly ICsvFileReaderService<VaccinationDataModel> _vaccinationDataReaderService;
        private readonly IVaccinationDataRepository _vaccinationDataRepository;
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly CsvFileHelper _csvFileHelper;
        private readonly CountryNameMapper _countryNameMapper;

        public VaccinationDataService(IOptions<CsvFileSettings> csvFileSettings,
                                      ICsvFileReaderService<VaccinationDataModel> vaccinationDataReaderService,
                                      IVaccinationDataRepository vaccinationDataRepository,
                                      IFileIntegrationRepository integrationRepository,
                                      CsvFileHelper csvFileHelper,
                                      IConfiguration configuration)
        {
            _csvFileSettings = csvFileSettings.Value;
            _vaccinationDataReaderService = vaccinationDataReaderService;
            _vaccinationDataRepository = vaccinationDataRepository;
            _integrationRepository = integrationRepository;
            _csvFileHelper = csvFileHelper;
            var mappingFilePath = configuration["JsonFiles:CountryNameMappingFile"];

            if (string.IsNullOrEmpty(mappingFilePath))
            {
                throw new InvalidOperationException("CountryNameMappingFilePath is missing in configuration.");
            }

            _countryNameMapper = new CountryNameMapper(mappingFilePath);
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
        public async Task<IEnumerable<VaccinationDataDto>> GetAllVaccinationDataAsync()
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            var vaccinationData = await _vaccinationDataRepository.GetAllVaccinationDataByMaxIntegrationIdAsync(csvFilename);
            return vaccinationData;
        }

        public async Task<VaccinationDataDto> GetVaccinationDataByCountryAsync(string? country)
        {
            var csvUrl = GetCsvUrl();

            var csvFilename = _csvFileHelper.ExtractFilename(csvUrl);

            _csvFileHelper.ValidateCsvFilename(csvFilename);

            // Map the input country name using the mapper
            string mappedCountryName = _countryNameMapper.MapCountryNameByValue(country);

            var vaccinationDataCountry = await _vaccinationDataRepository.GetVaccinationDataByMaxIntegrationIdAndCountryAsync(csvFilename, mappedCountryName);

            return new VaccinationDataDto
            {
                Region = country,
                TotalVaccineDoses = vaccinationDataCountry.TotalVaccineDoses,
                PersonsVaccinatedAtLeastOneDose = vaccinationDataCountry.PersonsVaccinatedAtLeastOneDose,
                PersonsVaccinatedWithCompletePrimarySeries = vaccinationDataCountry.PersonsVaccinatedWithCompletePrimarySeries
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