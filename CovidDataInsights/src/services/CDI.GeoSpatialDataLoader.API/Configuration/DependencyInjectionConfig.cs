using CDI.GeoSpatialDataLoader.API.Data.Repository;
using CDI.GeoSpatialDataLoader.API.Services;

namespace CDI.GeoSpatialDataLoader.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped<IGeoSpatialRepository, GeoSpatialRepository>();

            // Register Services
            services.AddScoped<IGeoSpatialService, GeoSpatialService>();
        }
    }
}