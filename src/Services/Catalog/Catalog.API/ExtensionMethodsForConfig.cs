namespace Catalog.API;

public static class ExtensionMethodsForConfig
{
    public static IServiceCollection RegisterApiDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services._registerSwagger();
        return services;
    }

    private static void _registerSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static WebApplication TurnOnSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    public static WebApplication MapActions(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
        return app;
    }
}