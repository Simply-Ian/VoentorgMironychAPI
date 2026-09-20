using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Catalog.Application.Handlers.GenericHandlers;

public class UpdateEntityCommandHandler<TContract, TEntity> 
(
    IRepository repo,
    IMapper mapper
) : IRequestHandler<UpdateEntityCommand<TContract, TEntity>, Unit>
    where TEntity : BaseEntity
    where TContract : class
{
    public async Task<Unit> Handle(UpdateEntityCommand<TContract, TEntity> request, CancellationToken cancellationToken)
    {
        var newEntity = await mapper.MapAsync<TEntity>(request.Contract);
        newEntity.Id = request.Id;
        await repo.SaveAsync(newEntity);
        return default;
    }
}