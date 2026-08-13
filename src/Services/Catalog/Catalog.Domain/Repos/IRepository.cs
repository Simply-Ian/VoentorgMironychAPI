using Catalog.Domain.Entities;

namespace Catalog.Domain.Repos;

public interface IRepository<T> where T: BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    delegate bool Selector(T item);
    /* TODO: если возможно, убрать селектор и сделать возврат IQueriable с тем, чтобы клиентский код мог сам
     определиться, как ему нужно отфильтровать/обработать результаты */
    Task<IEnumerable<T>> GetBySelectorAsync(Selector selector);
    /// <summary>
    /// Сохраняет новосозданный экземпляр сущности или обновляет уже существующий в БД
    /// </summary>
    /// <param name="item">Экземпляр сущности, который нужно сохранить</param>
    Task SaveAsync(T item);
    Task DeleteByIdAsync(Guid id);
    // TODO: если надо, добавить удаление по селектору
}