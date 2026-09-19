using Catalog.Application.Behaviors;
using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Application.Handlers.GenericHandlers;
using Catalog.Application.Queries.ItemQueries;
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
                // Не регистрирует обобщенные обработчики, потому что ограничение дженерик-параметра
                // where TContract: class слишком широкое (Камянецкий)
                config.RegisterServicesFromAssembly(typeof(CreateItemCommandHandler<,>).Assembly);
                
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ExceptionActionProcessorBehavior<,>));
                config.AddBehavior<
                    IPipelineBehavior<CreateEntityCommand<CreateItemContract, Item>, CreatedResult<Item>>,
                    CommandValidationBehavior<CreateEntityCommand<CreateItemContract, Item>, CreatedResult<Item>>
                >();
                config.AddBehavior<
                    IPipelineBehavior<UpdateEntityCommand<CreateItemContract, Item>, Unit>,
                    CommandValidationBehavior<UpdateEntityCommand<CreateItemContract, Item>, Unit>
                >();
                config.AddBehavior<
                    IPipelineBehavior<DeleteEntityCommand<Item>, Unit>,
                    CommandValidationBehavior<DeleteEntityCommand<Item>, Unit>
                >();
            }
        )
        #region Handlers
        .AddScoped<IRequestHandler<CreateEntityCommand<CreateItemContract, Item>, CreatedResult<Item>>, 
            CreateItemCommandHandler<CreateItemContract, Item>>()
        .AddScoped<IRequestHandler<UpdateEntityCommand<CreateItemContract, Item>, Unit>, 
            UpdateEntityCommandHandler<CreateItemContract, Item>>()
        .AddScoped<IRequestHandler<DeleteEntityCommand<Item>, Unit>, DeleteEntityCommandHandler<Item>>()
        #endregion
        
        #region Validators 
        .AddScoped<IValidator<CreateEntityCommand<CreateItemContract, Item>>, CreateItemCommandValidator>()
        .AddTransient<IValidator<GetItemsWithPaginationQuery>, GetItemsWithPaginationQueryValidator>()
        .AddTransient<IValidator<CommandWithId<Item>>, CommandWithIdValidator<Item>>()
        .AddTransient<IValidator<UpdateEntityCommand<CreateItemContract, Item>>, UpdateItemCommandValidator>()
        .AddTransient<IValidator<DeleteEntityCommand<Item>>, DeleteEntityCommandValidator<Item>>()
        .AddTransient<IValidator<CreateItemContract>, CreateItemContractValidator>()
        #endregion
        .ConfigureMapster()
        .AddTransient<IMapper, ServiceMapper>(); // ServiceMapper, а не Mapper, чтобы работал DI в методах маппинга
        
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