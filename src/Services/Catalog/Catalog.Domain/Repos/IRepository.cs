using System.Linq.Expressions;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repos;

public interface IRepository
{
    Task<IEnumerable<T>> GetAllAsync<T>() where T: BaseEntity;
    
    /// <returns>Экземпляр сущности T, обладающей указанным id, или null,
    /// если таковой не существует</returns>
    Task<T?> GetByIdAsync<T>(Guid id) where T: BaseEntity;
    Task<IEnumerable<T>> GetBySelectorAsync<T>(Expression<Func<T, bool>> selector) where T : BaseEntity;
    
    /// <summary>
    /// Сохраняет новосозданный экземпляр сущности или обновляет уже существующий в БД
    /// </summary>
    /// <param name="item">Экземпляр сущности, который нужно сохранить</param>
    Task SaveAsync<T>(T item) where T: BaseEntity;
    Task DeleteByIdAsync<T>(Guid id) where T: BaseEntity;
    // Если надо, можно добавить удаление по селектору
}