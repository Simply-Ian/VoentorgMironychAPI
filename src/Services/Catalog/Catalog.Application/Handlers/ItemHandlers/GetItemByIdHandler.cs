using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers.ItemHandlers;

public class GetItemByIdHandler(IRepository repo) : IRequestHandler<GetItemByIdQuery, SearchItemsResult>
{
    public async Task<SearchItemsResult> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetByIdAsync<Item>(request.Id);
        return result is null ? new([]) : new([result]);
    }
}