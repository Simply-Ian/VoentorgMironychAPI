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

public class CreateEntityCommandHandler<TDto, TEntity>
(
    IValidator<TDto> validator, 
    ILogger<CreateEntityCommandHandler<TDto, TEntity>> logger,
    IHostEnvironment environment,
    IRepository repo,
    IMapper mapper
) : IRequestHandler<CreateEntityCommand<TDto, TEntity>, CreatedResult<TEntity>> 
    where TEntity: BaseEntity
    where TDto : class
{
    public async Task<CreatedResult<TEntity>> Handle(CreateEntityCommand<TDto, TEntity> request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            if (environment.IsDevelopment()) 
                logger.LogWarning
                (
                    "Attempted to pass an invalid DTO. \n" +
                    "Class: {objClass}\n" +
                    "DTO contents: {DTO}\n" + 
                    "Validation issues: {ValidationErrors}",
                    typeof(TDto).Name, request.Dto, validationResult.Errors
                );
            throw new ValidationException(validationResult.Errors);
        }

        var newEntity = await mapper.From(request.Dto).AdaptToTypeAsync<TEntity>();
        await repo.SaveAsync(newEntity);
        return new CreatedResult<TEntity>(newEntity);
    }
}