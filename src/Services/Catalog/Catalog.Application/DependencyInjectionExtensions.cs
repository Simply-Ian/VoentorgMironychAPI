using Catalog.Application.Behaviors;
using Catalog.Application.Commands;
using Catalog.Application.DTO;
using Catalog.Application.Handlers.GenericHandlers;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Application.Validators;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using Mapster;
using MediatR;

namespace Catalog.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterApplicationDependencies
    (
        this IServiceCollection services,
        IConfiguration appConfiguration
    )
    {
        services.AddMediatR
        (
            config =>
            {
                config.LicenseKey = appConfiguration.GetSection("LicenseKeys").GetSection("MediatR").Value;
                config.RegisterGenericHandlers = true;
                config.RegisterServicesFromAssembly(typeof(CreateEntityCommandHandler<,>).Assembly);
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ExceptionActionProcessorBehavior<,>));
            }
        )
        .AddScoped<IRequestHandler<CreateEntityCommand<ItemDTO, Item>, CreatedResult<Item>>, CreateEntityCommandHandler<ItemDTO, Item>>()
        .AddScoped<IValidator<ItemDTO>, ItemDtoValidator>()
        .AddMapster();
        services.ConfigureMapster();
        
        return services;
    }

    private static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        services.AddSingleton(config);
        ConfigureMapster();
        return services;
    }
    
    private static void ConfigureMapster()
    {
        // Валидация гарантирует, что бренд и все категории существуют
        TypeAdapterConfig<ItemDTO, Item>
            .NewConfig()
            .AfterMappingAsync
            (async (dto, entity) =>
                {
                    var repo = MapContext.Current.GetService<IRepository>();
                    entity.Brand = dto.BrandId == null ? null : await repo.GetByIdAsync<Brand>(dto.BrandId.Value);
                    entity.Categories = (await Task.WhenAll(
                                dto.CategoryIds?.Select(
                                    repo.GetByIdAsync<Category>
                                ) ?? []
                            )
                        )!;
                }
            );
    }
}