using Catalog.Domain.Entities;

namespace Catalog.Application.Responses;

public record GetAllCategoriesResult(IEnumerable<Category> Result);