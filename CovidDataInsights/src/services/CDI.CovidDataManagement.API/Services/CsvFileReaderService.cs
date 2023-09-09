using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using CDI.WebAPI.Core.Utils;

namespace CDI.CovidDataManagement.API.Services
{
    public interface ICsvFileReaderService<T>
    {
        Task<(List<T>, int)> ReadCsvFile(string csvUrl);
    }
    public class CsvFileReaderService<T, TMap> : ICsvFileReaderService<T> where TMap : ClassMap<T>
    {
        private readonly HttpClient _httpClient;

        public CsvFileReaderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<T>, int)> ReadCsvFile(string csvUrl)
        {
            var records = new List<T>();
            int numberOfRows = 0;

            using (var response = await _httpClient.GetAsync(csvUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        Stream contentStream;

                        // Gzip decompression for VaccinationData and VaccinationMetaData files
                        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                        {
                            contentStream = new GZipStream(stream, CompressionMode.Decompress);
                        }
                        else
                        {
                            // Use original stream for WhoGlobalData and WhoGlobalTableData files
                            contentStream = stream;
                        }

                        using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = true
                        }))
                        {
                            csv.Context.RegisterClassMap<TMap>();
                            records = csv.GetRecords<T>().ToList();
                            numberOfRows = records.Count;
                        }
                    }
                }
                else
                {
                    throw new AppException($"Failed to fetch CSV from URL: {csvUrl}");
                }
            }

            return (records, numberOfRows);
        }
    }
}