var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterApiDependencies(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration);

var app = builder.Build();

app.TurnOnSwagger();
app.MapActions();

app.Run();