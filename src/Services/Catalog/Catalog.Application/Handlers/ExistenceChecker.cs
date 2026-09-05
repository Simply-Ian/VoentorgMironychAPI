using Catalog.Domain.Entities;
using Catalog.Domain.Repos;

namespace Catalog.Application.Handlers;

public class ExistenceChecker (IRepository repo)
{
    protected async Task<bool> _assureEntityExists<TEntity> (Guid id)
        where TEntity: BaseEntity => await repo.GetByIdAsync<TEntity>(id) != null;
}