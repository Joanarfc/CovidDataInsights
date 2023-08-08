using CDI.CovidDataManagement.API.Data;
using CDI.CovidDataManagement.API.Data.Repository;
using CDI.CovidDataManagement.API.Services;

namespace CDI.CovidDataManagement.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IFileIntegrationRepository, FileIntegrationRepository>();
            services.AddScoped<IVaccinationDataRepository, VaccinationDataRepository>();
            services.AddScoped<IVaccinationMetaDataRepository, VaccinationMetaDataRepository>();
            services.AddScoped<IWhoGlobalDataRepository, WhoGlobalDataRepository>();

            // Register context
            services.AddScoped<ApplicationDbContext>();

            // Register Services
            services.AddScoped<IFileIntegrationService, FileIntegrationService>();
        }
    }
}