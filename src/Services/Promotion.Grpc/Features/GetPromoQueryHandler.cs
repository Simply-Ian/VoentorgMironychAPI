using System.Data;
using Common.Exceptions;
using Common.Interfaces;
using Dapper;
using MediatR;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Features;

internal record GetPromoQuery(Guid CatalogItemId) : IQuery<Promo>;

internal class GetPromoQueryHandler(IDbConnection connection) : IRequestHandler<GetPromoQuery, Promo>
{
    // Сколько скидок может иметь один Item?
    private const string GetPromoByCatalogItemId = 
        """
        select * from promos 
        where CatalogItemId = uuid_to_bin(@CatalogItemId)
        limit 1
        """;

    public async Task<Promo> Handle(GetPromoQuery request, CancellationToken cancellationToken)
    {
        var result = await connection.QueryAsync<Promo?>(
            GetPromoByCatalogItemId, 
            new { CatalogItemId = request.CatalogItemId.ToString() }
        );
        return result.FirstOrDefault() ?? throw new NotFoundException($"Промоакция для элемента каталога с " +
                                                     $"Id={request.CatalogItemId} не найдена");
    }
}