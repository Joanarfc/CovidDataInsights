using CDI.CovidApp.MVC.Services;

namespace CDI.CovidApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Services
            services.AddHttpClient<ICovidDataService, CovidDataService>();
        }
    }
}