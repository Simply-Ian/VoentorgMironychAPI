using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public record DeleteEntityCommand<TEntity>(Guid Id) : IRequest<DeleteEntityByIdResult>
    where TEntity : BaseEntity;