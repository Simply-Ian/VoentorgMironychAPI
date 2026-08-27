using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers.ItemHandlers;

public class GetItemsByTitleHandler(IRepository repo) : IRequestHandler<GetItemsByTitleQuery, SearchItemsResult>
{
    public async Task<SearchItemsResult> Handle(GetItemsByTitleQuery request, CancellationToken cancellationToken)
        => new(await repo.GetBySelectorAsync<Item>(
                    i => !string.IsNullOrEmpty(i.Title) 
                         && i.Title.Contains(request.ProvidedTitle, StringComparison.OrdinalIgnoreCase)
                )
            );
}