using Catalog.Application.Commands;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Catalog.Application.Handlers.GenericHandlers;

public class CreateItemCommandHandler<TContract, TEntity>
(
    IValidator<TContract> validator, 
    ILogger<CreateItemCommandHandler<TContract, TEntity>> logger,
    IHostEnvironment environment,
    IRepository repo,
    IMapper mapper
) : IRequestHandler<CreateEntityCommand<TContract, TEntity>, CreatedResult<TEntity>> 
    where TEntity: BaseEntity
    where TContract : class
{
    public async Task<CreatedResult<TEntity>> Handle(CreateEntityCommand<TContract, TEntity> request, CancellationToken cancellationToken)
    {
        var newEntity = await mapper.From(request.Contract).AdaptToTypeAsync<TEntity>();
        await repo.SaveAsync(newEntity);
        return new CreatedResult<TEntity>(newEntity);
    }
}