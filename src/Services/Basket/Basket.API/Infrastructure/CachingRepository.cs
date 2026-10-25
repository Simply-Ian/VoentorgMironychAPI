using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Basket.API.Infrastructure;

/// <summary>
/// Обертка над репозиторием, которая пытается читать значение из кэша перед тем, как обратиться к БД, и пишет в кэш,
/// соответственно. Исключения, возникающие при работе с кэшем, перехватываются, т. к. в отличие от БД, проблемы кэша не
/// являются для приложения фатальными
/// </summary>
public class CachingRepository(
    ICartRepository repo, 
    IDistributedCache cache, 
    IOptions<DistributedCacheEntryOptions> opts,
    ILogger<CachingRepository> logger
) : ICartRepository
{
    public async Task<ShoppingCart> GetCartAsync(string accountName, CancellationToken ct)
    {
        ShoppingCart? cart;
        
        try
        {
            cart = await cache.GetAsync<ShoppingCart>(accountName, ct);
            if (cart is not null) return cart;
        }
        catch (Exception exc)
        {
            logger.LogWarning(exc, "Cache unavailable");
        }
        
        cart = await repo.GetCartAsync(accountName, ct);
        
        /* Если кэш внезапно станет недоступен, вызов метода задержит возврат результата пользователю, пока кэш не 
        станет доступен вновь (но не дольше, чем таймаут). При этом фоновая запись - не лучшее решение, т. к. такая
        задача не видна в метриках и не отменится при остановке приложения (Камянецкий). Компромисс - короткий таймаут
        подключения */
        await _storeCartInCacheAsync(accountName, cart);
        
        return cart;
    }

    private async Task _storeCartInCacheAsync(string accountName, ShoppingCart cart)
    {
        try
        {
            await cache.SetAsync(accountName, cart, opts.Value);
        }
        catch (Exception exc)
        {
            logger.LogWarning("Failed to store value in cache: {exc}", exc);
        }
    }

    public async Task RemoveCartAsync(string accountName, CancellationToken ct)
    {
        /*
         * Если сначала удалять из кэша, а потом - из БД, то в промежутке между удалениями другой поток может прочитать
         * запись из БД и положить ее ОБРАТНО В КЭШ, где она будет храниться до истечения TTL. На неатомарность удаления
         * забиваем, т. к. распределенная блокировка дороже самой проблемы, а паттерн transactional outbox в курсе не
         * рассматривается.
         */
        await repo.RemoveCartAsync(accountName, ct);
        try
        {
            await cache.RemoveAsync(accountName, ct);    
        }
        catch (Exception exc)
        {
            logger.LogWarning("Failed to remove value from cache: {exc}", exc);
        }
    }

    public async Task<ShoppingCart> StoreCartAsync(ShoppingCart cart, CancellationToken ct)
    {
        /* Писать в БД надо в первую очередь, т. к. если писать в кэш первым, то при падении БД в кэше останется запись,
         в БД никак не отраженная. */
        var result = await repo.StoreCartAsync(cart, ct);
        await _storeCartInCacheAsync(cart.AccountName, cart);
        return result;
    }
}