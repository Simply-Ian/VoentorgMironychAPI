using Catalog.Domain.Entities;

namespace Catalog.Application.Responses.ItemResponses;

public record GetAllItemsResult(IEnumerable<Item> Items);