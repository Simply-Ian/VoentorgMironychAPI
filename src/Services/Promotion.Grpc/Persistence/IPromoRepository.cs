using Promotion.Grpc.Models;

namespace Promotion.Grpc.Persistence;

/// <returns>
/// true, если запись с указанным Id уже существовала в БД (т. е. было выполнено обновление), иначе false
/// </returns>
public interface IPromoRepository
{
    public Task<bool> StoreAsync(Promo promo);
}