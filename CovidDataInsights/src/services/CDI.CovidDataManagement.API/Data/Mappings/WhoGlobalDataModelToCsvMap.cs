using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;

namespace CDI.CovidDataManagement.API.Data.Mappings
{
    public class WhoGlobalDataModelToCsvMap : ClassMap<WhoGlobalDataModel>
    {
        public WhoGlobalDataModelToCsvMap()
        {
            Map(m => m.DateReported).Name("Date_reported");
            Map(m => m.CountryCode).Name("Country_code");
            Map(m => m.Country).Name("Country");
            Map(m => m.WhoRegion).Name("WHO_region");
            Map(m => m.NewCases).Name("New_cases");
            Map(m => m.CumulativeCases).Name("Cumulative_cases");
            Map(m => m.NewDeaths).Name("New_deaths");
            Map(m => m.CumulativeDeaths).Name("Cumulative_deaths");
        }
    }
}