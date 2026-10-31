namespace Promotion.Grpc.Models;

public record Promo(Guid Id, Guid? CatalogItemId, string Title, decimal Value);