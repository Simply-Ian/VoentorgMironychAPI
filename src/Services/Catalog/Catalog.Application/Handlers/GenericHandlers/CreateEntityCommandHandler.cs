using Catalog.Application.Commands;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Catalog.Application.Handlers.GenericHandlers;

public class CreateEntityCommandHandler<TContract, TEntity>
(
    IValidator<TContract> validator, 
    ILogger<CreateEntityCommandHandler<TContract, TEntity>> logger,
    IHostEnvironment environment,
    IRepository repo,
    IMapper mapper
) : IRequestHandler<CreateEntityCommand<TContract, TEntity>, CreatedResult<TEntity>> 
    where TEntity: BaseEntity
    where TContract : class
{
    public async Task<CreatedResult<TEntity>> Handle(CreateEntityCommand<TContract, TEntity> request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Contract, cancellationToken);
        if (!validationResult.IsValid)
        {
            if (environment.IsDevelopment()) 
                logger.LogWarning
                (
                    "Attempted to pass an invalid DTO. \n" +
                    "Class: {objClass}\n" +
                    "DTO contents: {DTO}\n" + 
                    "Validation issues: {ValidationErrors}",
                    typeof(TContract).Name, request.Contract, validationResult.Errors
                );
            throw new ValidationException(validationResult.Errors);
        }

        var newEntity = await mapper.From(request.Contract).AdaptToTypeAsync<TEntity>();
        await repo.SaveAsync(newEntity);
        return new CreatedResult<TEntity>(newEntity);
    }
}