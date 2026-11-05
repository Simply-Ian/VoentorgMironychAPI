using System.Data;
using Dapper;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Persistence;

public class DapperPromoRepository(IDbConnection connection) : IPromoRepository
{
    public async Task<bool> StoreAsync(Promo promo)
    {
        var rowExists = await _checkKeyExists(promo.CatalogItemId);
        const string sql = 
            """
            insert into promos (CatalogItemId, Title, Value)
            values(@CatalogItemId, @Title, @Value)
            on duplicate key update
                                 CatalogItemId = @CatalogItemId,
                                 Title = @Title,
                                 Value = @Value
            """;
        await connection.ExecuteAsync(sql, promo);
        return rowExists;
    }

    private async Task<bool> _checkKeyExists(Guid key)
        => await connection.QueryFirstAsync<int>(
            "select count(*) from promos where CatalogItemId = @key",
            new { key = key }
            ) > 0;
}