using Catalog.Domain.Entities;

namespace Catalog.Application.Responses.CategoryResponses;

public record GetAllCategoriesResult(IEnumerable<Category> Categories);