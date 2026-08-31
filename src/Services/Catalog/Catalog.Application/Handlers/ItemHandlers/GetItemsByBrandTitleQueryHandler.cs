using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers.ItemHandlers;

public class GetItemsByBrandTitleQueryHandler(IRepository repo) : IRequestHandler<GetItemsByBrandTitleQuery, SearchItemsResult>
{
    public async Task<SearchItemsResult> Handle(GetItemsByBrandTitleQuery request, CancellationToken cancellationToken)
        => new 
        ( 
            await repo.GetBySelectorAsync<Item>
            (
                i => i.Brand != null 
                     && !string.IsNullOrEmpty(i.Brand.Title)
                     && i.Brand.Title.Contains(request.BrandTitle)
            )
        );
}