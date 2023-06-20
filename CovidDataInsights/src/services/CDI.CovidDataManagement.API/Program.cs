using CDI.CovidDataManagement.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();

var app = builder.Build();

app.UseApiConfiguration();

app.Run();
