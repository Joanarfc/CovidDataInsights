using CDI.CovidDataManagement.API.Data;
using CDI.CovidDataManagement.API.Data.Mappings;
using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Models;
using CDI.CovidDataManagement.API.Services;

namespace CDI.CovidDataManagement.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            // Register repositories
            services.AddScoped<IFileIntegrationRepository, FileIntegrationRepository>();
            services.AddScoped<IVaccinationDataRepository, VaccinationDataRepository>();
            services.AddScoped<IVaccinationMetaDataRepository, VaccinationMetaDataRepository>();
            services.AddScoped<IWhoGlobalDataRepository, WhoGlobalDataRepository>();
            services.AddScoped<IWhoGlobalTableDataRepository, WhoGlobalTableDataRepository>();

            // Register context
            services.AddScoped<ApplicationDbContext>();

            // Register Services
            services.AddScoped<IFileIntegrationService, FileIntegrationService>();
            services.AddScoped<ICsvFileReaderService<VaccinationDataModel>, CsvFileReaderService<VaccinationDataModel, VaccinationDataModelToCsvMap>>();
            services.AddScoped<ICsvFileReaderService<VaccinationMetaDataModel>, CsvFileReaderService<VaccinationMetaDataModel, VaccinationMetaDataModelToCsvMap>>();
            services.AddScoped<ICsvFileReaderService<WhoGlobalDataModel>, CsvFileReaderService<WhoGlobalDataModel, WhoGlobalDataModelToCsvMap>>();
            services.AddScoped<ICsvFileReaderService<WhoGlobalTableDataModel>, CsvFileReaderService<WhoGlobalTableDataModel, WhoGlobalTableDataModelToCsvMap>>();
        }
    }
}