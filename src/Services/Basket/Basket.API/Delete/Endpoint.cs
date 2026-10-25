using Carter;
using MediatR;

namespace Basket.API.Delete;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/cart/{accountName}",
            async (string accountName, ISender sender) =>
            {
                var command = new DeleteCartCommand(accountName);
                await sender.Send(command);
                return Results.NoContent();
            }
        )
        .WithName("Delete a previously saved cart")
        .ProducesProblem(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Delete a previously saved cart, given username")
        .WithDescription("Throws 404 if an account with the given name does not exist. The only validation rule is " +
                         "that accountName must be non-empty.");
    }
}