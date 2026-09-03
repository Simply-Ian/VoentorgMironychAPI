using Catalog.Application.Commands;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.GenericHandlers;

public class UpdateEntityCommandHandler<TContract, TEntity> 
(
    IValidator<TContract> validator, 
    ILogger<UpdateEntityCommandHandler<TContract, TEntity>> logger,
    IHostEnvironment environment,
    IRepository repo,
    IMapper mapper
) : IRequestHandler<UpdateEntityCommand<TContract, TEntity>> 
    where TEntity : BaseEntity
    where TContract : class
{
    public async Task Handle(UpdateEntityCommand<TContract, TEntity> request, CancellationToken cancellationToken)
    {
        await _validateRequest(request);
        var newEntity = await mapper.MapAsync<TEntity>(request.Contract);
        newEntity.Id = request.Id;
        await repo.SaveAsync(newEntity);
    }
    
    private async Task _validateRequest(UpdateEntityCommand<TContract, TEntity> request)
    {
        ValidationResult validationResult;
        if (! await _assureEntityExists(request.Id))
            validationResult = new([new ValidationFailure(
                "id", 
                $"Specified entity of type {nameof(TEntity)} does not exist"
            )]);
        else validationResult = await validator.ValidateAsync(request.Contract);
        
        if (!validationResult.IsValid)
        {
            if (environment.IsDevelopment())
            {
                string message = "";
                if (validationResult.Errors[0].PropertyName == "id")
                    message = "Attempted to handle invalid UPDATE request";
                else message = "Attempted to pass an invalid DTO. \n" +
                               "Class: {objClass}\n" +
                               "DTO contents: {DTO}\n" + 
                               "Validation issues: {ValidationErrors}";
                logger.LogWarning
                (
                    message,
                    typeof(TContract).Name, request.Contract, validationResult.Errors
                );
            }
            throw new ValidationException(validationResult.Errors);
        }
    }

    private async Task<bool> _assureEntityExists(Guid id) => await repo.GetByIdAsync<TEntity>(id) != null;
}