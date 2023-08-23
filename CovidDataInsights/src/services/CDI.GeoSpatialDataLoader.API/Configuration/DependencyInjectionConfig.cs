using CDI.GeoSpatialDataLoader.API.Data.Repository;

namespace CDI.GeoSpatialDataLoader.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IGeoSpatialRepository, GeoSpatialRepository>();
        }
    }
}