namespace Catalog.Domain.Specifications;

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
    // 0-based index
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