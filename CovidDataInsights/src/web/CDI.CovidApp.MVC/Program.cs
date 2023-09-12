using CDI.CovidApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddMvcConfiguration(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();