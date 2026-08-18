using System.Linq.Expressions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Marten;

namespace Catalog.Infrastructure;

public class MartenRepository(IDocumentStore store) :
    IRepository,
    IAsyncDisposable
{
    private readonly IDocumentSession _session = store.IdentitySession();
    
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T: BaseEntity => await _session.Query<T>().ToListAsync();
    
    public async Task<T?> GetByIdAsync<T>(Guid id) where T: BaseEntity => await _session.LoadAsync<T>(id);
    
    public async Task<IEnumerable<T>> GetBySelectorAsync<T>(Expression<Func<T, bool>> selector) where T: BaseEntity => 
        await _session.Query<T>().Where(selector).ToListAsync();
    
    public async Task SaveAsync<T>(T item) where T: BaseEntity
    {
        _session.Store(item);
        await _session.SaveChangesAsync();
    }
    
    public async Task DeleteByIdAsync<T>(Guid id) where T: BaseEntity
    {
        _session.Delete<T>(id);
        await _session.SaveChangesAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _session.DisposeAsync();
    }
}