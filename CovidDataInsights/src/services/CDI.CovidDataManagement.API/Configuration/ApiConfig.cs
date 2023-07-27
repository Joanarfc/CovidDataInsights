using CDI.CovidDataManagement.API.Data;
using CDI.CovidDataManagement.API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.Configure<CsvFileSettings>(configuration.GetSection("CsvFiles"));
        }
        public static void UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}