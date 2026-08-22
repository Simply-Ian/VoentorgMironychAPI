using System.Linq.Expressions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Marten;

namespace Catalog.Infrastructure;

public class MartenRepository(IDocumentSession session) :
    IRepository
{
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T: BaseEntity => await session.Query<T>().ToListAsync();
    
    public async Task<T?> GetByIdAsync<T>(Guid id) where T: BaseEntity => await session.LoadAsync<T>(id);
    
    public async Task<IEnumerable<T>> GetBySelectorAsync<T>(Expression<Func<T, bool>> selector) where T: BaseEntity => 
        await session.Query<T>().Where(selector).ToListAsync();
    
    public async Task SaveAsync<T>(T item) where T: BaseEntity
    {
        session.Store(item);
        await session.SaveChangesAsync();
    }
    
    public async Task DeleteByIdAsync<T>(Guid id) where T: BaseEntity
    {
        session.Delete<T>(id);
        await session.SaveChangesAsync();
    }
}