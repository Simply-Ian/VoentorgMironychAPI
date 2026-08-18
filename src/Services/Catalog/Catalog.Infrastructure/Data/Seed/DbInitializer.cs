using Catalog.Domain.Entities;
using Common;
using Marten;
using Marten.Schema;

namespace Catalog.Infrastructure.Data.Seed;

file static class SeedData
{
    public static Brand[] Brands = 
    [
        new() { Id = Guid.Parse("b0000000-0000-0000-0000-000000000000"), Title = "БТК Групп"},
        new() { Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"), Title = "Альянс-пошив"},
        new() { Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"), Title = "Техкрим"},
        new() { Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"), Title = "BIZON"},
        new() { Id = Guid.Parse("b0000000-0000-0000-0000-000000000005"), Title = "Армия России"}
    ];

    public static Category[] Categories = 
    [
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000000"), Title = "Одежда"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"), Title = "Обувь"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"), Title = "Средства самообороны"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"), Title = "ВКПО 4.0"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"), Title = "Уценка"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"), Title = "Продовольствие"},
        new() { Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"), Title = "Шевроны и нашивки"}
    ];
    
    public static Item[] Items = 
    [
        new Wear
        {
            Id = Guid.Parse("d0000000-0000-0000-0000-000000000000"),
            Title = "Летний комплект ВКПО 4.0",
            Brand = Brands.First(b => b.Title == "БТК Групп"),
            Categories = 
            [
                Categories.First(c => c.Title == "Одежда"), 
                Categories.First(c => c.Title == "ВКПО 4.0")
            ],
            FullDescription = "Летний комплект полевого обмундирования от лучшего производителя в России. " +
                              "Высокое качество, плотная и прочная ткань, не выцветает на солнце и от стирки." +
                              "В комплекте: китель, брюки, головной убор (кепка), брючный ремень",
            ShortDescription = "Летний комплект ВКПО (БТК, 4 предмета)",
            Material = Material.SyntheticFabric,
            ImageUrl = new UriBuilder(scheme: "https", host: CommonConstants.StaticContentHostname, port: 80, pathValue: "images/vkpo4_0_btk.png").Uri,
            Price = 5500
        },
        new Wear
        {
            Id = Guid.Parse("d0000000-0000-0000-0000-000000000001"),
            Title = "Летний комплект ВКПО 4.0",
            Brand = Brands.First(b => b.Title == "Альянс-пошив"),
            Categories = 
            [
                Categories.First(c => c.Title == "Одежда"), 
                Categories.First(c => c.Title == "ВКПО 4.0")
            ],
            FullDescription = "Идеальная форма для жаркой погоды. Легкая дышащая ткань облегчает циркуляцию воздуха и снижает вес комплекта." +
                              "В комплекте: китель, брюки, головной убор (кепка), брючный ремень",
            ShortDescription = "Летний комплект ВКПО (Альянс, 4 предмета)",
            Material = Material.CottonFabric,
            ImageUrl = new UriBuilder(scheme: "https", host: CommonConstants.StaticContentHostname, port: 80, pathValue: "images/vkpo4_0_alliance.png").Uri,
            Price = 3000
        },
        // TODO: как добавить поля "Причина уценки" и "Исходная цена" (или "Величина уценки")?
        new Wear
        {
            Id = Guid.Parse("d0000000-0000-0000-0000-000000000002"),
            Title = "Берцы \"Утка\" демисезонные",
            Brand = Brands.First(b => b.Title == "BIZON"),
            Categories = 
            [
                Categories.First(c => c.Title == "Обувь"), 
                Categories.First(c => c.Title == "ВКПО 4.0"),
                Categories.First(c => c.Title == "Уценка")
            ],
            FullDescription = "Легкие демисезонные берцы. Молния на боку позволяет удобно и быстро надевать и снимать их, тонкая натуральная кожа" +
                              "оберегает ноги в жару, протектор на подшве улучшает сцепление с поверхностью.",
            ShortDescription = "Демисезонные берцы (нат. кожа)",
            Material = Material.Leather,
            ImageUrl = new UriBuilder(scheme: "https", host: CommonConstants.StaticContentHostname, port: 80, pathValue: "images/bertsy_utka.png").Uri,
            Price = 4500
        },
        new ()
        {
            Id = Guid.Parse("d0000000-0000-0000-0000-000000000003"),
            Title = "Перцовый баллончик \"Шпага\"",
            Brand = Brands.First(b => b.Title == "Техкрим"),
            Categories = 
            [
                Categories.First(c => c.Title == "Средства самообороны")
            ],
            FullDescription = "Струйный перцовый баллончик \"Шпага\" - классика самообороны. Двойное действие, дальность" +
                              "до 5 м, можно применять в помещении. Снабжен защитой от случайного нажатия.",
            ShortDescription = "Струнный баллончик OD + CS",
            ImageUrl = new UriBuilder(scheme: "https", host: CommonConstants.StaticContentHostname, port: 80, pathValue: "images/shpaga.png").Uri,
            Price = 2000,
            IsAgeRestricted = true
        },
        new ()
        {
            Id = Guid.Parse("d0000000-0000-0000-0000-000000000004"),
            Title = "Индивидуальный рацион питания (сухпаек)",
            Brand = Brands.First(b => b.Title == "Армия России"),
            Categories = 
            [
                Categories.First(c => c.Title == "Продовольствие")
            ],
            FullDescription = "Армейский рацион питания в ассортименте. Большие сроки годности, все варианты комплектования. " +
                              "Идеальный вариант для выезда на природу, похода, подарка.",
            ShortDescription = "Армейский сухпаек в ассортименте",
            ImageUrl = new UriBuilder(scheme: "https", host: CommonConstants.StaticContentHostname, port: 80, pathValue: "images/irp.png").Uri,
            Price = 900
        }
    ];
}

public class DbInitializer : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();
        if (!await session.Query<Brand>().AnyAsync(token: cancellation)) session.Store(SeedData.Brands);
        if (!await session.Query<Category>().AnyAsync(token: cancellation)) session.Store(SeedData.Categories);
        if (!await session.Query<Item>().AnyAsync(token: cancellation)) session.Store(SeedData.Items);
        await session.SaveChangesAsync(cancellation);
    }
}