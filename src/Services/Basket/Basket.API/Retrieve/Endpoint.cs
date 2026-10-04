using Basket.API.Models;
using Carter;
using MediatR;

namespace Basket.API.Retrieve;

public record GetCartResponse(ShoppingCart Cart)
{
    public static implicit operator GetCartResponse(GetCartResult handlerResult) 
        => new (handlerResult.Cart);
}

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/cart/{accountName}", async (string accountName, ISender sender) =>
        {
            var query = new GetCartQuery(accountName);
            GetCartResponse response = await sender.Send(query);
            return Results.Ok(response);
        })
        .WithName("Retrieve a saved cart")
        .Produces<GetCartResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Retrieved a previously saved cart, given username")
        .WithDescription("Relies on the service's own storage, so in case the user " +
                         "has been deleted it's still possible to retrieve his last saved cart.");
    }
}