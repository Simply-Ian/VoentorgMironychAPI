using Basket.API.Models;
using Common.Exceptions;
using Marten;

namespace Basket.API.Infrastructure;

public class MartenCartRepository(IDocumentSession session) : ICartRepository
{
    public async Task<ShoppingCart> GetCartAsync(string accountName, CancellationToken ct) 
        => await session.LoadAsync<ShoppingCart>(accountName, ct) 
           ?? throw new NotFoundException($"ShoppingCart for account {accountName} not found");

    public async Task RemoveCartAsync(string accountName, CancellationToken ct)
    {
        if (!await session.CheckExistsAsync<ShoppingCart>(accountName, ct)) 
            throw new NotFoundException($"ShoppingCart for account {accountName} not found");
        session.Delete<ShoppingCart>(accountName);
        await session.SaveChangesAsync(ct);
    }

    public async Task<ShoppingCart> StoreCartAsync(ShoppingCart cart, CancellationToken ct)
    {
        session.Store(cart);
        await session.SaveChangesAsync(ct);
        return cart;
    }
}