using Catalog.Domain.Entities;

namespace Catalog.Domain.Specifications;

public record PaginatedResponse<T>(
    IReadOnlyList<T> Contents,
    int PageSize,
    int PageIndex,
    int PagesCount
) where T : BaseEntity
{
    public static int CalculatePagesCount(int entriesCount, int pageSize) => (entriesCount + pageSize - 1) / pageSize;
}