using Catalog.Application.Commands;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.GenericHandlers;

public class DeleteEntityCommandHandler<TEntity>(IRepository repo, ILogger<DeleteEntityCommandHandler<TEntity>> logger): 
    ExistenceChecker(repo), IRequestHandler<DeleteEntityCommand<TEntity>, DeleteEntityByIdResult>
    where TEntity : BaseEntity
{
    public async Task<DeleteEntityByIdResult> Handle(DeleteEntityCommand<TEntity> request, 
        CancellationToken cancellationToken)
    {
        if (!await _assureEntityExists<TEntity>(request.Id)) 
            return new (
                false, new ValidationException("Entity not found")
            );
        try
        {
            await repo.DeleteByIdAsync<TEntity>(request.Id);
        }
        catch (Exception exc)
        {
            logger.LogError($"An exception occurred while attempting to delete {nameof(TEntity)} with id {request.Id}:" +
                            $"{exc.Message}");
            return new(false, exc);
        }
        return new (true);
    }
}