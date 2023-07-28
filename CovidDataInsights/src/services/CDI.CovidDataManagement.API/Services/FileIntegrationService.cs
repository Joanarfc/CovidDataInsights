using CDI.CovidDataManagement.API.Data.Mappings;
using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CDI.CovidDataManagement.API.Services
{
    public interface IFileIntegrationService
    {
        Task IntegrateCsvDataAsync();
    }
    public class FileIntegrationService : IFileIntegrationService
    {
        // Dependencies
        private readonly IFileIntegrationRepository _integrationRepository;
        private readonly IVaccinationDataRepository _vaccinationDataRepository;
        private readonly CsvFileSettings _csvFileSettings;

        public FileIntegrationService(IFileIntegrationRepository integrationRepository,
                                     IOptions<CsvFileSettings> csvFileSettings,
                                     IVaccinationDataRepository vaccinationDataRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _integrationRepository = integrationRepository;
            _vaccinationDataRepository = vaccinationDataRepository;
        }

        public async Task IntegrateCsvDataAsync()
        {
            // Get the folder path and filenames from settings
            var csvFolderPath = _csvFileSettings.CsvPath;

            var vaccinationDataFilename = _csvFileSettings?.VaccinationDataFile;

            // Combine the folder path and filenames to get the full file paths
            var vaccinationDataFile = Path.Combine(csvFolderPath ?? string.Empty, vaccinationDataFilename ?? string.Empty);

            // Process the VaccinationData CSV file if it exists
            if (File.Exists(vaccinationDataFile))
            {
                // Read the CSV file and get the number of records
                var (vaccinationData, numberOfRows) = ReadCsvFile<VaccinationDataModel, VaccinationDataModelToCsvMap>(vaccinationDataFile);
                var rowsIntegrated = vaccinationData.Count();

                if (vaccinationDataFilename != null)
                {
                    // Create an integration model and add it to the repository
                    var integrationModel = CreateIntegrationModel(vaccinationDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    // Set the IntegrationId (FK) for each record and add them to the VaccinationData repository
                    foreach (var data in vaccinationData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    // Add VaccinationData in database
                    await _vaccinationDataRepository.AddVaccinationDataRangeAsync(vaccinationData);
                }
            }
        }

        // Reads the CSV file and returns the data records along with the number of rows in the file
        private (List<T>, int) ReadCsvFile<T, TMap>(string csvFilePath) where TMap : ClassMap<T>
        {
            var records = new List<T>();
            int numberOfRows = 0;

            // Read the CSV file using CsvHelper library
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                records = csv.GetRecords<T>().ToList();
                numberOfRows = records.Count;
            }

            return (records, numberOfRows);
        }

        // Creates an integration model object
        private IntegrationModel CreateIntegrationModel(string fileName, int numberOfRows, int rowsIntegrated)
        {
            return new IntegrationModel
            {
                IntegrationTimestamp = DateTime.UtcNow,
                FileName = fileName,
                NumberOfRows = numberOfRows,
                RowsIntegrated = rowsIntegrated
            };
        }
    }
}