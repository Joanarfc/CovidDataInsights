using CDI.CovidDataManagement.API.Data;
using CDI.CovidDataManagement.API.Data.Repository;

namespace CDI.CovidDataManagement.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IFileIntegrationRepository, FileIntegrationRepository>();
            services.AddScoped<IVaccinationDataRepository, VaccinationDataRepository>();

            // Register context
            services.AddScoped<ApplicationDbContext>();
        }
    }
}