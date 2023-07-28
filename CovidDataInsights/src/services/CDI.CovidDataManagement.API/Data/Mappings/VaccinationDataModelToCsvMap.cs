using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;
using System.Globalization;

namespace CDI.CovidDataManagement.API.Data.Mappings
{
    public sealed class VaccinationDataModelToCsvMap : ClassMap<VaccinationDataModel>
    {
        public VaccinationDataModelToCsvMap()
        {
            Map(m => m.Country).Name("COUNTRY");
            Map(m => m.ISO3).Name("ISO3");
            Map(m => m.WhoRegion).Name("WHO_REGION");
            Map(m => m.DataSource).Name("DATA_SOURCE");
            Map(m => m.DateUpdated).Name("DATE_UPDATED");
            Map(m => m.TotalVaccinations).Name("TOTAL_VACCINATIONS")
                .TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.PersonsVaccinated_1Plus_Dose).Name("PERSONS_VACCINATED_1PLUS_DOSE")
                .TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty); ;
            Map(m => m.TotalVaccinations_Per100).Name("TOTAL_VACCINATIONS_PER100");
            Map(m => m.PersonsVaccinated_1Plus_Dose_Per100).Name("PERSONS_VACCINATED_1PLUS_DOSE_PER100");
            Map(m => m.PersonsLastDose).Name("PERSONS_LAST_DOSE").TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty); ;
            Map(m => m.PersonsLastDosePer100).Name("PERSONS_LAST_DOSE_PER100");
            Map(m => m.VaccinesUsed).Name("VACCINES_USED").Default(string.Empty);
            Map(m => m.FirstVaccineDate).Name("FIRST_VACCINE_DATE")
                .TypeConverterOption.Format("yyyy-MM-dd")
                .TypeConverterOption.NullValues(string.Empty);
            Map(m => m.NumberVaccinesTypesUsed).Name("NUMBER_VACCINES_TYPES_USED").Default(string.Empty);
            Map(m => m.PersonsBoosterAddDose).Name("PERSONS_BOOSTER_ADD_DOSE")
                .TypeConverterOption.NumberStyles(NumberStyles.AllowThousands)
                .TypeConverterOption.NullValues(string.Empty); ;
            Map(m => m.PersonsBoosterAddDose_Per100).Name("PERSONS_BOOSTER_ADD_DOSE_PER100");
        }
    }
}