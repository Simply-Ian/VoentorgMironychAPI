using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Infrastructure;

/// <summary>
/// Обертка над репозиторием, которая пытается читать значение из кэша перед тем, как обратиться к БД, и пишет в кэш,
/// соответственно. Исключения, возникающие при работе с кэшем, перехватываются, т. к. в отличие от БД, проблемы кэша не
/// являются для приложения фатальными
/// </summary>
public class CachingRepository(
    ICartRepository repo, 
    IDistributedCache cache, 
    DistributedCacheEntryOptions opts,
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
            logger.LogWarning("Cache unavailable: {exc}", exc);
        }
        
        cart = await repo.GetCartAsync(accountName, ct);
        
        /* Если кэш внезапно станет недоступен, вызов метода задержит возврат результата пользователю, пока кэш не 
        станет доступен вновь (но не дольше, чем таймаут). Поэтому намеренно запускаем запись в кэш в фоне и возвращаем
        результат, не дожидаясь окончания */
        Task.Run(async () => await _storeCartInCache(accountName, cart));
        
        return cart;
    }

    private async Task _storeCartInCache(string accountName, ShoppingCart cart)
    {
        try
        {
            /* Есть ли смысл делать это асинхронно? Работает ли планировщик асинхронных заданий в фоновых потоках из
            thread pool-а? */
            await cache.SetAsync(accountName, cart, opts);
        }
        catch (Exception exc)
        {
            logger.LogWarning("Failed to store value in cache: {exc}", exc);
        }
    }

    public async Task RemoveCartAsync(string accountName, CancellationToken ct)
    {
        /*
         * Если удаление одновременно и из кэша, и из БД не будет атомарным, то может случиться так, что после удаления
         * из кэша текущий поток будет остановлен планировщиком и другой поток сможет считать удаленную запись из БД.
         * В случае Redis это можно решить, написав блокировку вручную с помощью SETNX (методы чтения проверяют блокировку
         * и входят в активное ожидание, если заблокировано; метод удаления блокирует кэш, удаляет из БД, удаляет из 
         * кэша, снимает блокировку), но мы здесь используем IDistributedCache, не привязываясь к деталям реализации, 
         * поэтому удаление оставим неатомарным.
         */
        try
        {
            await cache.RemoveAsync(accountName, ct);    
        }
        catch (Exception exc)
        {
            logger.LogWarning("Failed to remove value from cache: {exc}", exc);
        }
        await repo.RemoveCartAsync(accountName, ct);
    }

    public async Task<ShoppingCart> StoreCartAsync(ShoppingCart cart, CancellationToken ct)
    {
        /* Если кэш внезапно станет недоступен, вызов метода задержит возврат результата пользователю, пока кэш не
        станет доступен вновь (но не дольше, чем таймаут). Поэтому намеренно запускаем запись в кэш в фоне и возвращаем
        результат, не дожидаясь окончания */
        Task.Run(async () => await _storeCartInCache(cart.AccountName, cart));
        return await repo.StoreCartAsync(cart, ct);
    }
}