using Catalog.Domain.Entities;
using Catalog.Domain.Repos;

namespace Catalog.Application.Handlers;

public class ExistenceChecker
{
    public async Task<bool> AssureEntityExists<TEntity> (Guid id, IRepository repo)
        where TEntity: BaseEntity => await repo.GetByIdAsync<TEntity>(id) != null;
}