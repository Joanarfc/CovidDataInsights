using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CDI.CovidDataManagement.API.Services
{
    public interface ICsvFileReaderService<T>
    {
        (List<T>, int) ReadCsvFile(string csvFilePath);
    }
    public class CsvFileReaderService<T, TMap> : ICsvFileReaderService<T> where TMap : ClassMap<T>
    {
        public (List<T>, int) ReadCsvFile(string csvFilePath)
        {
            var records = new List<T>();
            int numberOfRows = 0;

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                records = csv.GetRecords<T>().ToList();
                numberOfRows = records.Count;
            }

            return (records, numberOfRows);
        }
    }
}