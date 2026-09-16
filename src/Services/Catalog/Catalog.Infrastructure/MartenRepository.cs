using System.Linq.Expressions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Catalog.Domain.Specifications;
using Marten;
using Marten.Linq;

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
    public async Task<PaginatedResponse<T>> GetPaginatedResponse<T>(QueryArgs args) where T : BaseEntity
    {
        var query = _makeQuery(session.Query<T>(), args);
        var queriedContent = await query.ToListAsync();
        
        return new PaginatedResponse<T>(
            queriedContent, 
            args.PageSize, 
            args.PageIndex, 
            PaginatedResponse<T>.CalculatePagesCount(await CountAsync<T>(), args.PageSize)
        );
    }

    private static IQueryable<Item> _makeQuery(IQueryable<Item> query, QueryArgs args)
    {
        query = query.Where(item => (
                       string.IsNullOrEmpty(args.SearchString) 
                    || (!string.IsNullOrEmpty(item.Title) && item.Title.ToLower().Contains(args.SearchString.ToLower()))
                    || (!string.IsNullOrEmpty(item.ShortDescription) && item.ShortDescription.ToLower().Contains(args.SearchString.ToLower()))
                    || (!string.IsNullOrEmpty(item.FullDescription) && item.FullDescription.ToLower().Contains(args.SearchString.ToLower()))
                )
                && (args.BrandId == null || (item.Brand != null && item.Brand.Id == args.BrandId))
                && (args.CategoryId == null || item.Categories.Any(cat => cat.Id == args.CategoryId))
            );
        /* Marten не проверяет истинность членов выражения, когда транслирует в SQL; вместо атрибутов программных объектов
        подставляет их значения; поэтому если мы напишем в предыдущем Where-clause что-то вроде
        `... && (args.PriceFilter != null || <цена товара в промежутке>)`
        и на вход придет null, то мы получим разыменование null со всеми вытекающими */
        if (args.MinPrice != null) query = query.Where(item => args.MinPrice <= item.Price);
        if (args.MaxPrice != null) query = query.Where(item => item.Price <= args.MaxPrice);
        
        // Validation ensures that specified FieldToSortBy contains the name of an existing property
        if (args.FieldToSortBy?.ToLower() == nameof(Item.Title).ToLower())
            query = args.Order == QueryArgs.SortOrder.Ascending ? 
                query.OrderBy(item => item.Title) 
                : query.OrderByDescending(item => item.Title);
        else if (args.FieldToSortBy?.ToLower() == nameof(Item.Price).ToLower())
            query = args.Order == QueryArgs.SortOrder.Ascending ?
                query.OrderBy(item => item.Price)
                : query.OrderByDescending(item => item.Price);
        else query = query.OrderBy(item => item.Id);
        return _specifyPagination(query, args);
    }

    private static IQueryable<T> _makeQuery<T>(IQueryable<T> query, QueryArgs args) where T : BaseEntity
        => typeof(T) == typeof(Item)? (IQueryable<T>)_makeQuery((IQueryable<Item>)query, args) 
            : _specifyPagination(query.OrderBy(entry => entry.Id), args);

    private static IQueryable<T> _specifyPagination<T>(IQueryable<T> query, QueryArgs args)
    where T : BaseEntity 
        => query.Skip(args.PageIndex * args.PageSize)
            .Take(args.PageSize);
}