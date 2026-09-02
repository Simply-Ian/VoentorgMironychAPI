using Catalog.Domain.Entities;

namespace Catalog.Application.Responses.GenericResponses;

public record CreatedResult<TEntity>(TEntity CreatedEntity) where TEntity: BaseEntity;