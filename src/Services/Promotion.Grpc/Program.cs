using System.Data;
using System.Reflection;
using FluentValidation;
using MySqlConnector;
using Promotion.Grpc.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .RegisterGrpc()
    .RegisterMediatR(builder.Configuration)
    .AddScoped<IDbConnection>(
        _ => new MySqlConnection(builder.Configuration.GetConnectionString("MySql"))
    )
    .AddScoped<IPromoRepository, DapperPromoRepository>()
    .ConfigureMapster()
    .AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));

var app = builder.Build();
await app.SeedDatabase();
app.MapEndpoints();
app.Run();