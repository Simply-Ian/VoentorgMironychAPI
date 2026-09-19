using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public record DeleteEntityCommand<TEntity>(Guid Id) : CommandWithId<TEntity>(Id), IRequest<Unit>
    where TEntity : BaseEntity;