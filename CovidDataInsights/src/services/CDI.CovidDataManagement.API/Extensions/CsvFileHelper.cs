namespace CDI.CovidDataManagement.API.Extensions
{
    public class CsvFileHelper
    {
        public string ExtractFilename(string csvUrl)
        {
            string csvFilename;

            if (!string.IsNullOrEmpty(csvUrl))
            {
                csvFilename = Path.GetFileName(csvUrl);
            }
            else
            {
                throw new InvalidOperationException("CSV URL is null or empty.");
            }

            return csvFilename;
        }
        public void ValidateCsvFilename(string csvFilename)
        {
            if (string.IsNullOrEmpty(csvFilename))
            {
                throw new FileNotFoundException("CSV filename is missing.");
            }
        }
    }
}