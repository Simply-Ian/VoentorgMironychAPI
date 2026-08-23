using Catalog.Application.Queries;
using Catalog.Application.Responses.BrandResponses;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using MediatR;

namespace Catalog.Application.Handlers.BrandHandlers;

public class GetAllBrandsQueryHandler(IRepository repo) : IRequestHandler<GetAllBrandsQuery, GetAllBrandsResult>
{
    public async Task<GetAllBrandsResult> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        => new(await repo.GetAllAsync<Brand>());
}