using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;
using MediatR;

namespace Catalog.Application.Handlers.ItemHandlers;

public class GetItemsWithPaginationQueryHandler(IRepository repo, IValidator<GetItemsWithPaginationQuery> validator) 
    : IRequestHandler<GetItemsWithPaginationQuery, GetItemsWithPaginationResult>
{
    public async Task<GetItemsWithPaginationResult> Handle(GetItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        // TODO: обрабатывать ситуацию, когда строка поиска null
        return await repo.GetPaginatedResponse<Item>(request.Args);
    }
}