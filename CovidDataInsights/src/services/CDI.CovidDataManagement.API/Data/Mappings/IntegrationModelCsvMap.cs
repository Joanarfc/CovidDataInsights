using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;

namespace CDI.CovidDataManagement.API.Data.Mappings
{
    public sealed class IntegrationModelCsvMap : ClassMap<IntegrationModel>
    {
        public IntegrationModelCsvMap()
        {
            Map(m => m.IntegrationTimestamp).Name("IntegrationTimestamp");
            Map(m => m.FileName).Name("FileName");
            Map(m => m.NumberOfRows).Name("NumberOfRows");
            Map(m => m.RowsIntegrated).Name("RowsIntegrated");
        }
    }
}