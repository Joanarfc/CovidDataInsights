using CDI.CovidDataManagement.API.Models;
using CsvHelper.Configuration;

namespace CDI.CovidDataManagement.API.Data.Mappings
{
    public sealed class VaccinationMetaDataModelToCsvMap : ClassMap<VaccinationMetaDataModel>
    {
        public VaccinationMetaDataModelToCsvMap()
        {
            Map(m => m.ISO3).Name("ISO3");
            Map(m => m.VaccineName).Name("VACCINE_NAME");
            Map(m => m.ProductName).Name("PRODUCT_NAME");
            Map(m => m.CompanyName).Name("COMPANY_NAME");
            Map(m => m.AuthorizationDate).Name("AUTHORIZATION_DATE");
            Map(m => m.StartDate).Name("START_DATE");
            Map(m => m.EndDate).Name("END_DATE");
            Map(m => m.Comment).Name("COMMENT").Default(string.Empty);
            Map(m => m.DataSource).Name("DATA_SOURCE");
        }
    }
}