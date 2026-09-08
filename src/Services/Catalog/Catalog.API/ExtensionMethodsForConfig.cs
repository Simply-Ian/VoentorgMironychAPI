using Microsoft.OpenApi;

namespace Catalog.API;

public static class ExtensionMethodsForConfig
{
    public static IServiceCollection RegisterApiDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services._registerSwagger();
        services.AddControllers();
        services.AddLogging();
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new(1, 0);
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }

    private static void _registerSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc
            (
                "v1", 
                new OpenApiInfo
                {
                    Title = "Catalog API", 
                    Version = "v1"
                }
            );
            config.SwaggerDoc
            (
                "v2", 
                new OpenApiInfo
                {
                    Title = "Catalog API", 
                    Version = "v2"
                }
            );
            
            config.EnableAnnotations();
        });
    }

    public static WebApplication TurnOnSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });
        }

        return app;
    }

    public static WebApplication MapActions(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}