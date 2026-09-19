using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public record UpdateEntityCommand<TContract, TEntity>(Guid Id, TContract Contract) : CommandWithId<TEntity>(Id), IRequest<Unit>
    where TEntity : BaseEntity
    where TContract : class;