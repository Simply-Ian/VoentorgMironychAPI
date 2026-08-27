using Catalog.Domain.Entities;

namespace Catalog.Application.Responses.BrandResponses;

public record GetAllBrandsResult(IEnumerable<Brand> Brands);