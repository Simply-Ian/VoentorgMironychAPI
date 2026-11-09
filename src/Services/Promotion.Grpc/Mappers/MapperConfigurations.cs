using Mapster;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Mappers;

public class MapperConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Promo, PromoDto>()
            .Map(dst => dst.Value, src => new DecimalValue(src.Value));
    }
}