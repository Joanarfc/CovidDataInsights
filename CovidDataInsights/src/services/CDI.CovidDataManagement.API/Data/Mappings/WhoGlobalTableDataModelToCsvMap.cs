using CDI.CovidDataManagement.API.Extensions;
using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;
using System.Globalization;

namespace CDI.CovidDataManagement.API.Data.Mappings
{
    public class WhoGlobalTableDataModelToCsvMap : ClassMap<WhoGlobalTableDataModel>
    {
        public WhoGlobalTableDataModelToCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.WhoRegion).Name("WHO Region");
            Map(m => m.CasesCumulativeTotal).Name("Cases - cumulative total")
                .TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.CasesCumulativeTotal_Per100000_Population).Name("Cases - cumulative total per 100000 population")
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.CasesNewlyReportedInLast7Days).Name("Cases - newly reported in last 7 days");
            Map(m => m.CasesNewlyReportedInLast7Days_Per100000_Population).Name("Cases - newly reported in last 7 days per 100000 population")
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.CasesNewlyReportedInLast24Hours).Name("Cases - newly reported in last 24 hours");
            Map(m => m.DeathsCumulativeTotal).Name("Deaths - cumulative total")
                .TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.DeathsCumulativeTotal_Per100000_Population).Name("Deaths - cumulative total per 100000 population")
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.DeathsNewlyReportedInLast7Days).Name("Deaths - newly reported in last 7 days");
            Map(m => m.DeathsNewlyReportedInLast7Days_Per100000_Population).Name("Deaths - newly reported in last 7 days per 100000 population")
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.DeathsNewlyReportedInLast24Hours).Name("Deaths - newly reported in last 24 hours");
        }
    }
}