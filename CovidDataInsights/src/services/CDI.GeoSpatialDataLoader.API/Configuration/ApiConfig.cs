using CDI.GeoSpatialDataLoader.API.Data;
using CDI.GeoSpatialDataLoader.API.Extensions;
using CDI.WebAPI.Core.Middleware;
using Microsoft.EntityFrameworkCore;

namespace CDI.GeoSpatialDataLoader.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.Configure<GeoJsonFileSettings>(configuration.GetSection("GeoJsonFile"));
        }
        public static void UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}