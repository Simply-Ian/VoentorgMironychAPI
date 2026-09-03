using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public record CreateEntityCommand<TContract, TEntity>(TContract Contract) : IRequest<CreatedResult<TEntity>> 
    where TEntity : BaseEntity
    where TContract : class;