using Promotion.Grpc.Models;

namespace Promotion.Grpc.Persistence;

public static class SeedData
{
    public static List<Promo> Promos = [
        new(
            Guid.NewGuid(), 
            Guid.Parse("d0000000-0000-0000-0000-000000000001"), 
            "Скидка 200 р на летнюю форму",
            200m
        ),
        new(
            Guid.NewGuid(), 
            Guid.Parse("01a067ef-048d-7d24-98f9-58ede1f72204"), 
            "Скидка 500 р на средства самообороны",
            500m
        )
    ];
}