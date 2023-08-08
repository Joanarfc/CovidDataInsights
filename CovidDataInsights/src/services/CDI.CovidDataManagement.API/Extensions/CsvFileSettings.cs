namespace CDI.CovidDataManagement.API.Extensions
{
    public class CsvFileSettings
    {
        public string? CsvPath { get; set; }
        public string? VaccinationDataFile { get; set; }
        public string? VaccinationMetadataFile { get; set; }

        public string? GlobalDataFile { get; set; }
    }
}