using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllCategoriesQueryHandler (IRepository repo) : 
    IRequestHandler<GetAllCategoriesQuery, GetAllCategoriesResult>
{
    public async Task<GetAllCategoriesResult> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        => new(await repo.GetAllAsync<Category>());
}