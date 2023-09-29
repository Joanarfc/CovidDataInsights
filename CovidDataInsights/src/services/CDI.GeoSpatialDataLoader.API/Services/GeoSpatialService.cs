using CDI.GeoSpatialDataLoader.API.Data.Repository;
using CDI.GeoSpatialDataLoader.API.Extensions;
using CDI.GeoSpatialDataLoader.API.Models;
using CDI.WebAPI.Core.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CDI.GeoSpatialDataLoader.API.Services
{
    public interface IGeoSpatialService
    {
        Task<IEnumerable<GeoSpatialModel>> GetAllAsync();
        Task<string?> LoadAndSaveDataAsync();
    }
    public class GeoSpatialService : IGeoSpatialService
    {
        private readonly GeoJsonFileSettings _csvFileSettings;
        private readonly ILogger _logger;
        private readonly IGeoSpatialRepository _geoSpatialRepository;

        public GeoSpatialService(IOptions<GeoJsonFileSettings> csvFileSettings,
                                 ILogger<GeoSpatialRepository> logger,
                                 IGeoSpatialRepository geoSpatialRepository)
        {
            _csvFileSettings = csvFileSettings.Value;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into GeoSpatialRepository");
            _logger.LogDebug(1, "GeoSpatialRepository has been constructed");
            _geoSpatialRepository = geoSpatialRepository;
        }

        public async Task<IEnumerable<GeoSpatialModel>> GetAllAsync()
        {
            _logger.LogInformation("GetAllAsync method started.");

            return await _geoSpatialRepository.GetAllAsync();
        }

        public async Task<string?> LoadAndSaveDataAsync()
        {
            _logger.LogInformation("LoadAndSaveDataAsync method started.");

            // Check if the table is empty before we load the data. If not empty, skip the extract transform and load process
            var dbResult = _geoSpatialRepository.GetCountriesCountFromDB();
            _logger.LogInformation("Countries table result: {dbResult}", dbResult);

            if (dbResult > 0)
            {
                _logger.LogInformation("Data already loaded.");
                return "Data is already loaded.";
            }
            else if (dbResult == 0)
            {
                _logger.LogInformation("No data available in Countries table.");
            }

            // Load data from the Geo JSON file
            var geoJsonData = await GetAndReadGeoJsonFileContentAsync();

            if (geoJsonData != null)
            {
                // Process and save the Geo JSON data
                await ProcessAndSaveDataAsync(geoJsonData);
                return geoJsonData;
            }
            else
            {
                _logger.LogInformation("The file does not exist.");
                throw new AppException("The file does not exist.");
            }
        }

        private async Task<string?> GetAndReadGeoJsonFileContentAsync()
        {
            // Get the folder path and filenames from settings
            var geoJsonFolderPath = _csvFileSettings.GeoJsonPath;
            _logger.LogInformation("Geo json folder path: {geojsonFolderPath}", geoJsonFolderPath);

            var geoJsonDataFilename = _csvFileSettings?.GeoJsonDataFile;
            _logger.LogInformation("Geo json file name: {geoJsonDataFilename}", geoJsonDataFilename);

            // Combine the folder path and filenames to get the full file paths
            var geoJsonDataFile = Path.Combine(geoJsonFolderPath ?? string.Empty, geoJsonDataFilename ?? string.Empty);
            _logger.LogInformation("Geo json file path: {geoJsonDataFile}", geoJsonDataFile);

            if (File.Exists(geoJsonDataFile))
            {
                return await File.ReadAllTextAsync(geoJsonDataFile);
            }
            throw new FileNotFoundException("The file does not exist.");
        }

        private async Task ProcessAndSaveDataAsync(string geoJsonData)
        {
            dynamic? jsonObj = JsonConvert.DeserializeObject(geoJsonData);

            if (jsonObj == null)
            {
                throw new AppException("Failed to deserialize JSON data.");
            }

            int numberOfFeatures = jsonObj["features"].Count;
            _logger.LogInformation("Number of features in the Geo JSON file: {numberOfFeatures}", numberOfFeatures);

            foreach (var feature in jsonObj["features"])
            {
                // Extract values from the Geo JSON file
                var geoSpatial = ExtractGeoSpatialModel(feature);

                _logger.LogInformation("Geo json data retrieved: " +
                    $"Feature CLA = {geoSpatial.Featurecla} | " +
                    $"Sovereignt = {geoSpatial.Sovereignt} | " +
                    $"Type = {geoSpatial.Type} | " +
                    $"Admin = {geoSpatial.Admin} | " +
                    $"Name Long = {geoSpatial.NameLong} | " +
                    $"Formal EN = {geoSpatial.FormalEN} | " +
                    $"Name EN = {geoSpatial.NameEN} | " +
                    $"Coordinates = {geoSpatial.Coordinates}");

                _geoSpatialRepository.AddGeoSpatialData(geoSpatial);

                var rowsAffected = await _geoSpatialRepository.SaveChangesAsync();
                _logger.LogInformation("Number of rows added into 'Countries' table: {rowsAffected}", rowsAffected);
            }

            var dbTotalRows = _geoSpatialRepository.GetCountriesCountFromDB();
            _logger.LogInformation("Total number of rows added into 'Countries' table: {dbTotalRows}", dbTotalRows);
        }

        private static GeoSpatialModel ExtractGeoSpatialModel(dynamic feature)
        {
            // Extract values from the Geo JSON feature
            var properties = feature["properties"];
            var geometry = feature["geometry"]["coordinates"].ToString(Formatting.None);

            return new GeoSpatialModel
            {
                Featurecla = properties["featurecla"],
                Sovereignt = properties["SOVEREIGNT"],
                Type = properties["TYPE"],
                Admin = properties["ADMIN"],
                NameLong = properties["NAME_LONG"],
                FormalEN = properties["FORMAL_EN"],
                NameEN = properties["NAME_EN"],
                Coordinates = geometry
            };
        }
    }
}