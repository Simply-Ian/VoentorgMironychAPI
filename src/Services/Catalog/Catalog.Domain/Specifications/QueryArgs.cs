namespace Catalog.Domain.Specifications;

/// <param name="PageSize">Must be greater than 0</param>
/// <param name="MinPrice">Must not exceed MaxPrice</param>
/// <param name="Order">0 stands for Ascending, 1 for Descending</param>
/// <param name="PageIndex">Indices of pages are 0-based</param>
/// <param name="FieldToSortBy">Currently supported Title and Price. Property names can be passed in any case, they are
/// cast to lowercase before the comparison</param>
public record QueryArgs 
(
    int PageSize,
    string? SearchString,
    Guid? BrandId,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? FieldToSortBy,
    QueryArgs.SortOrder Order = QueryArgs.SortOrder.Ascending,
    int PageIndex = 0
)
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    private const int MaxPageSize = 20;
    public int PageSize { get; } = Math.Min(MaxPageSize, PageSize);
}