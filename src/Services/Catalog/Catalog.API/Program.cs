using Catalog.API.Middlewares;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterApiDependencies(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GracefulExceptionHandling>();
app.TurnOnSwagger();
app.MapActions();

app.Run();