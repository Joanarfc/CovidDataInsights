using System.Text.Json;

namespace CDI.Core.DomainObjects
{
    public class CountryNameMapper
    {
        private readonly Dictionary<string, string> _countryNameMapping;

        public CountryNameMapper(string mappingFilePath)
        {
            if (string.IsNullOrEmpty(mappingFilePath))
            {
                throw new InvalidOperationException("Mapping file path is missing.");
            }

            _countryNameMapping = LoadCountryNameMappingFile(mappingFilePath);
        }

        public Dictionary<string, string> LoadCountryNameMappingFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            // If mapping file is not found, return empty dictionary
            return new Dictionary<string, string>();
        }

        // Map country names to a common format using the key in the dictionary
        public string MapCountryNameByKey(string inputName)
        {
            if (_countryNameMapping.ContainsKey(inputName))
            {
                return _countryNameMapping[inputName];
            }
            return inputName;
        }
        // Map country names to a common format using the value in the dictionary
        public string MapCountryNameByValue(string inputName)
        {
            foreach (var kvp in _countryNameMapping)
            {
                if (kvp.Value == inputName)
                {
                    // Return the key associated with the input value
                    return kvp.Key;
                }
            }
            return inputName;
        }
    }
}