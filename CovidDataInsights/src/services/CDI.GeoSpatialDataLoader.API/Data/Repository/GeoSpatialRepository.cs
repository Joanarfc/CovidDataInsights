using CDI.GeoSpatialDataLoader.API.Extensions;
using CDI.GeoSpatialDataLoader.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CDI.GeoSpatialDataLoader.API.Data.Repository
{
    public interface IGeoSpatialRepository
    {
        Task<IEnumerable<GeoSpatialModel>> GetAll();
    }
    public class GeoSpatialRepository : IGeoSpatialRepository
    {
        private readonly ApplicationDataContext _context;
        private readonly GeoJsonFileSettings _csvFileSettings;

        public GeoSpatialRepository(ApplicationDataContext context, 
                                    IOptions<GeoJsonFileSettings> csvFileSettings)
        {
            _context = context;
            _csvFileSettings = csvFileSettings.Value;
        }

        public async Task<IEnumerable<GeoSpatialModel>> GetAll()
        {
            SaveData();

            if (_context.Countries != null)
            {
                return await _context.Countries.ToListAsync();
            }
            else
            {
                return Enumerable.Empty<GeoSpatialModel>();
            }
        }

        public void SaveData()
        {
            // Check if the table is empty before we load the data. If not empty, skip the extract transform and load process
            var db_result = _context.Countries?.ToList();

            if (db_result?.Count == 0)
            {
                Console.WriteLine("No data in Countries table");

                // Get the folder path and filenames from settings
                var geojsonFolderPath = _csvFileSettings.GeoJsonPath;

                var geoJsonDataFilename = _csvFileSettings?.GeoJsonDataFile;

                // Combine the folder path and filenames to get the full file paths
                var geoJsonDataFile = Path.Combine(geojsonFolderPath ?? string.Empty, geoJsonDataFilename ?? string.Empty);

                if (File.Exists(geoJsonDataFile))
                {
                    var geoJSON = File.ReadAllText(geoJsonDataFile);

                    if (geoJSON != null)
                    {
                        dynamic? jsonObj = JsonConvert.DeserializeObject(geoJSON);

                        if (jsonObj != null)
                        {
                            foreach (var feature in jsonObj["features"])
                            {
                                // Extract values from the file object using the fields
                                string str_featurecla = feature["properties"]["featurecla"];
                                string str_sovereignt = feature["properties"]["SOVEREIGNT"];
                                string str_type = feature["properties"]["TYPE"];
                                string str_admin = feature["properties"]["ADMIN"];
                                string str_namelong = feature["properties"]["NAME_LONG"];
                                string str_formal_en = feature["properties"]["FORMAL_EN"];
                                string str_name_en = feature["properties"]["NAME_EN"];
                                string str_geometry = feature["geometry"]["coordinates"].ToString(Newtonsoft.Json.Formatting.None);

                                // Load the data into the table in database
                                GeoSpatialModel geoSpatial = new()
                                {
                                    Featurecla = str_featurecla,
                                    Sovereignt = str_sovereignt,
                                    Type = str_type,
                                    Admin = str_admin,
                                    NameLong = str_namelong,
                                    FormalEN = str_formal_en,
                                    NameEN = str_name_en,
                                    Coordinates = str_geometry
                                };

                                _context.Countries?.Add(geoSpatial);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The file does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Data loaded.");
            }
        }
    }
}