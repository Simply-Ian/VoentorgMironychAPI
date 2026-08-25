using Catalog.Domain.Entities;

namespace Catalog.Application.Responses;

public record GetAllBrandsResult(IEnumerable<Brand> Brands);