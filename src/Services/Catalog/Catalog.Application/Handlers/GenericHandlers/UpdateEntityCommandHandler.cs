using Catalog.Application.Commands;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.GenericHandlers;

public class UpdateEntityCommandHandler<TContract, TEntity> 
(
    ILogger<UpdateEntityCommandHandler<TContract, TEntity>> logger,
    IHostEnvironment environment,
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
        return Unit.Value;
    }
}