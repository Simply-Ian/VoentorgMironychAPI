using Catalog.Application.DTO;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public record CreateEntityCommand<TDto, TEntity>(TDto Dto) : IRequest<CreatedResult<TEntity>> 
    where TEntity : BaseEntity
    where TDto : class;