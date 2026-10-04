using Basket.API.Models;
using Carter;
using MediatR;

namespace Basket.API.Save;

public record SaveCartRequest(ShoppingCart Cart);
public record SaveCartResponse(string AccountName);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/cart", async (SaveCartRequest request, ISender sender) =>
        {
            var command = new SaveCartCommand(request.Cart);
            var result = await sender.Send(command);
            var response = new SaveCartResponse(result.AccountName);
            return Results.Created($"/cart/{response.AccountName}", response);
        })
        .WithName("Save cart")
        .Produces<SaveCartResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Save a new cart or update an existing one")
        .WithDescription("Performs upsert, but does not check whether the user exists. Returns the account name " +
                         "specified in the request");
    }
}