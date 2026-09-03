using Catalog.Application.Behaviors;
using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Application.Handlers.GenericHandlers;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Application.Validators;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using Mapster;
using MapsterMapper;
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
        .AddScoped<IRequestHandler<CreateEntityCommand<CreateItemContract, Item>, CreatedResult<Item>>, 
            CreateEntityCommandHandler<CreateItemContract, Item>>()
        .AddScoped<IRequestHandler<UpdateEntityCommand<CreateItemContract, Item>>, 
            UpdateEntityCommandHandler<CreateItemContract, Item>>()
        .AddScoped<IValidator<CreateItemContract>, CreateItemContractValidator>()
        .ConfigureMapster()
        .AddTransient<IMapper, ServiceMapper>();
        
        return services;
    }

    private static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        services.AddSingleton(config);
        _createConfigurations(config);
        return services;
    }
    
    private static void _createConfigurations(TypeAdapterConfig config)
    {
        // Валидация гарантирует, что бренд и все категории существуют
            config.NewConfig<CreateItemContract, Item>()
            .AfterMappingAsync
            (async (dto, entity) =>
                {
                    var repo = MapContext.Current.GetService<IRepository>();
                    entity.Brand = dto.BrandId == null ? null : await repo.GetByIdAsync<Brand>(dto.BrandId.Value);
                    entity.Categories = (await Task.WhenAll(
                                dto.CategoryIds?.Select(
                                    id => repo.GetByIdAsync<Category>(id)
                                ) ?? []
                            )
                        )!;
                }
            );
    }
}