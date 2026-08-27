using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers.ItemHandlers;

public class GetAllItemsQueryHandler(IRepository repo) : IRequestHandler<GetAllItemsQuery, GetAllItemsResult>
{
    public async Task<GetAllItemsResult> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        => new(await repo.GetAllAsync<Item>());
}