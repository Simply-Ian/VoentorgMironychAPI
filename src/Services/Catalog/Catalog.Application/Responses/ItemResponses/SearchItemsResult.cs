using Catalog.Domain.Entities;

namespace Catalog.Application.Responses.ItemResponses;

public record SearchItemsResult(IEnumerable<Item> Items);