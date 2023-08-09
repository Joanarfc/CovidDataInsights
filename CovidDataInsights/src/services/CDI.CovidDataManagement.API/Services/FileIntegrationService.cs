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
        private readonly IVaccinationMetaDataRepository _vaccinationMetaDataRepository;
        private readonly IWhoGlobalDataRepository _whoGlobalDataRepository;
        private readonly IWhoGlobalTableDataRepository _whoGlobalTableDataRepository;
        private readonly CsvFileSettings _csvFileSettings;

        public FileIntegrationService(IFileIntegrationRepository integrationRepository,
                                     IOptions<CsvFileSettings> csvFileSettings,
                                     IVaccinationDataRepository vaccinationDataRepository,
                                     IVaccinationMetaDataRepository vaccinationMetaDataRepository,
                                     IWhoGlobalDataRepository whoGlobalDataRepository,
                                     IWhoGlobalTableDataRepository whoGlobalTableDataRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _integrationRepository = integrationRepository;
            _vaccinationDataRepository = vaccinationDataRepository;
            _vaccinationMetaDataRepository = vaccinationMetaDataRepository;
            _whoGlobalDataRepository = whoGlobalDataRepository;
            _whoGlobalTableDataRepository = whoGlobalTableDataRepository;
        }

        public async Task IntegrateCsvDataAsync()
        {
            // Get the folder path and filenames from settings
            var csvFolderPath = _csvFileSettings.CsvPath;

            var vaccinationDataFilename = _csvFileSettings?.VaccinationDataFile;
            var vaccinationMetaDataFilename = _csvFileSettings?.VaccinationMetadataFile;
            var whoGlobalDataFilename = _csvFileSettings?.GlobalDataFile;
            var whoGlobalTableDataFilename = _csvFileSettings?.GlobalTableDataFile;

            // Combine the folder path and filenames to get the full file paths
            var vaccinationDataFile = Path.Combine(csvFolderPath ?? string.Empty, vaccinationDataFilename ?? string.Empty);
            var vaccinationMetaDataFile = Path.Combine(csvFolderPath ?? string.Empty, vaccinationMetaDataFilename ?? string.Empty);
            var whoGlobalDataFile = Path.Combine(csvFolderPath ?? string.Empty, whoGlobalDataFilename ?? string.Empty);
            var whoGlobalTableDataFile = Path.Combine(csvFolderPath ?? string.Empty, whoGlobalTableDataFilename ?? string.Empty);

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

            // Process the VaccinationMetadata CSV file if it exists
            if (File.Exists(vaccinationMetaDataFile))
            {
                // Read the CSV file and get the number of records
                var (vaccinationMetaData, numberOfRows) = ReadCsvFile<VaccinationMetaDataModel, VaccinationMetaDataModelToCsvMap>(vaccinationMetaDataFile);
                var rowsIntegrated = vaccinationMetaData.Count();

                if (vaccinationMetaDataFilename != null)
                {
                    // Create an integration model and add it to the repository
                    var integrationModel = CreateIntegrationModel(vaccinationMetaDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    // Set the IntegrationId (FK) for each record and add them to the VaccinationMetaData repository
                    foreach (var data in vaccinationMetaData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    // Add VaccinationMetaData in database
                    await _vaccinationMetaDataRepository.AddVaccinationMetaDataRangeAsync(vaccinationMetaData);
                }
            }

            // Process the WhoGlobalData CSV file if it exists
            if (File.Exists(whoGlobalDataFile))
            {
                // Read the CSV file and get the number of records
                var (whoGlobalData, numberOfRows) = ReadCsvFile<WhoGlobalDataModel, WhoGlobalDataModelToCsvMap>(whoGlobalDataFile);
                var rowsIntegrated = whoGlobalData.Count();

                if (whoGlobalDataFilename != null)
                {
                    // Create an integration model and add it to the repository
                    var integrationModel = CreateIntegrationModel(whoGlobalDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    // Set the IntegrationId (FK) for each record and add them to the VaccinationMetaData repository
                    foreach (var data in whoGlobalData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    // Add VaccinationMetaData in database
                    await _whoGlobalDataRepository.AddWhoGlobalDataRangeAsync(whoGlobalData);
                }
            }

            // Process the WhoGlobalTableData CSV file if it exists
            if (File.Exists(whoGlobalTableDataFile))
            {
                // Read the CSV file and get the number of records
                var (whoGlobalTableData, numberOfRows) = ReadCsvFile<WhoGlobalTableDataModel, WhoGlobalTableDataModelToCsvMap>(whoGlobalTableDataFile);
                var rowsIntegrated = whoGlobalTableData.Count();

                if (whoGlobalTableDataFilename != null)
                {
                    // Create an integration model and add it to the repository
                    var integrationModel = CreateIntegrationModel(whoGlobalTableDataFilename, numberOfRows, rowsIntegrated);
                    await _integrationRepository.AddAsync(integrationModel);

                    // Set the IntegrationId (FK) for each record and add them to the VaccinationMetaData repository
                    foreach (var data in whoGlobalTableData)
                    {
                        data.IntegrationId = integrationModel.Id;
                    }

                    // Add VaccinationMetaData in database
                    await _whoGlobalTableDataRepository.AddWhoGlobalTableDataRangeAsync(whoGlobalTableData);
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