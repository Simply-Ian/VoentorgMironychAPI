using Mapster;
using Promotion.Grpc.Features;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Mappers;

internal class MapperConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<GetPromoRequest, GetPromoQuery>()
            .Map(dst => dst.CatalogItemId, src => Guid.Parse(src.CatalogItemId));
        
        config
            .NewConfig<Promo, PromoDto>()
            .Map(dst => dst.CatalogItemId, src => src.CatalogItemId.ToString());
    }
}