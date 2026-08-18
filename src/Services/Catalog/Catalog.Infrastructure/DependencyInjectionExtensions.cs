using Catalog.Domain.Repos;
using Catalog.Infrastructure.Data.Seed;
using Marten;

namespace Catalog.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterInfrastructureDependencies
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMarten(opts =>
        {
            opts.Connection(configuration.GetConnectionString("CatalogDb")!);
            opts.DatabaseSchemaName = "catalog_service";
        })
        .InitializeWith<DbInitializer>();
        services.AddScoped<IRepository, MartenRepository>();
        return services;
    }
}