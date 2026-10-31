using Dapper;
using System.Data;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Persistence;

public static class DbExtensions
{
    /// <summary>
    /// Нужно запускать внутри try...catch, самостоятельно он не обрабатывает ошибки
    /// </summary>
    public static async Task SeedPromosAsync(IDbConnection connection)
    {
        const string schemaName = "promotion-db";
        const string tableName = "promos";
        if (connection.State != ConnectionState.Open) connection.Open();
        if (await TableExists(connection, schemaName, tableName)) return;
        await _createPromosTable(connection);
        
        foreach (var promo in SeedData.Promos)
            await _insertPromoRow(connection, promo);
    }

    public static async Task<bool> TableExists(IDbConnection connection, string schemaName, string tableName)
    {
        const string query =
            """
            select count(*)
            from information_schema.tables
            where table_schema = @SchemaName and table_name = @TableName
            """;
        var result = await connection.QueryFirstAsync<int>(
            query, 
            new { SchemaName = schemaName, TableName = tableName }
        );
        return result > 0;
    }

    private static async Task _createPromosTable(IDbConnection connection)
    {
        const string sql =
        """
            create table promos (
                Id binary(36) primary key,
                CatalogItemId char(36),
                Title text not null,
                Value decimal(8, 2) not null check(Value >= 0)
            );
            create index ix_promos_id on promos(Id);
            create index ix_promos_cat_item_id on promos(CatalogItemId);
        """;
        await connection.ExecuteAsync(sql);
    }

    private static async Task _insertPromoRow(IDbConnection connection, Promo promo)
    {
        const string sql =
            """
            insert into promos (Id, CatalogItemId, Title, Value) 
            values (uuid_to_bin(@Id), @CatalogItemId, @Title, @Value) 
            """;
        await connection.ExecuteAsync(sql, promo);
    }
}