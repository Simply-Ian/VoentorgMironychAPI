using Basket.API.Models;

namespace Basket.API.Infrastructure;

public interface ICartRepository
{
    Task<ShoppingCart> GetCartAsync(string accountName, CancellationToken ct);
    Task RemoveCartAsync(string accountName, CancellationToken ct);
    Task<ShoppingCart> StoreCartAsync(ShoppingCart shoppingCart, CancellationToken ct);
}