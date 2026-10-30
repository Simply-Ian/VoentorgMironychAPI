using System.Data;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .RegisterGrpc()
    .RegisterMediatR(builder.Configuration)
    .AddScoped<IDbConnection>(
        _ => new MySqlConnection(builder.Configuration.GetConnectionString("MySql"))
    );

var app = builder.Build();

app.MapEndpoints();
app.Run();