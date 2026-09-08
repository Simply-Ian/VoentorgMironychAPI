using Catalog.Domain.Entities;
using Catalog.Domain.Specifications;

namespace Catalog.Application.Responses.ItemResponses;

public record GetItemsWithPaginationResult(PaginatedResponse<Item> Items)
{
    public static implicit operator GetItemsWithPaginationResult(PaginatedResponse<Item> items) => new(items);
}