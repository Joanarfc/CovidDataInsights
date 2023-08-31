using CDI.GeoSpatialDataLoader.API.Configuration;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info($"Log Directory: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logging", DateTime.Now.ToString("yyyyMMdd"))}");
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.AddApiConfiguration(builder.Configuration);
    builder.Services.AddSwaggerConfiguration();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.RegisterServices();

    var app = builder.Build();

    app.UseSwaggerConfiguration();
    app.UseApiConfiguration();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, $"Stopped program because an exception occurred {exception}");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}