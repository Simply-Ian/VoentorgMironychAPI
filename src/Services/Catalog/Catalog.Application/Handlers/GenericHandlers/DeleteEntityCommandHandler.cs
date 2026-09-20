using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.GenericHandlers;

public class DeleteEntityCommandHandler<TEntity>(IRepository repo, ILogger<DeleteEntityCommandHandler<TEntity>> logger): 
    IRequestHandler<DeleteEntityCommand<TEntity>, Unit>
    where TEntity : BaseEntity
{
    public async Task<Unit> Handle(DeleteEntityCommand<TEntity> request, 
        CancellationToken cancellationToken)
    {
        try
        {
            await repo.DeleteByIdAsync<TEntity>(request.Id);
        }
        catch (Exception exc)
        {
            logger.LogError($"An exception occurred while attempting to delete {nameof(TEntity)} with id {request.Id}:" +
                            $"{exc.Message}");
        }
        return default;
    }
}