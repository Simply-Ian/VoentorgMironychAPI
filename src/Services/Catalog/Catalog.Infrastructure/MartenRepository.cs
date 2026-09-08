using System.Linq.Expressions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Catalog.Domain.Specifications;
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

    public async Task<int> CountAsync<T>() where T : BaseEntity => await session.Query<T>().CountAsync();
    public async Task<PaginatedResponse<T>> GetPaginatedResponse<T>(int pageIndex, int pageSize) where T : BaseEntity
    {
        var query = session.Query<T>();
        var queriedContent = await query
            .OrderBy(entry => entry.Id)
            .Skip(pageIndex*pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedResponse<T>(
            queriedContent, 
            pageSize, 
            pageIndex, 
            PaginatedResponse<T>.CalculatePagesCount(await query.CountAsync(), pageSize)
        );
    }
}