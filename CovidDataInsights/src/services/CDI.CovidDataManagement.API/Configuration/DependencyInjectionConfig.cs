using CDI.CovidDataManagement.API.Data;

namespace CDI.CovidDataManagement.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            // Register context
            services.AddScoped<ApplicationDbContext>();
        }
    }
}