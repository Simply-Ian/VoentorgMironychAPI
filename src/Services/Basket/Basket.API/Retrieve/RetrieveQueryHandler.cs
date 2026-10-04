using Basket.API.Infrastructure;
using Basket.API.Models;
using Common.Interfaces;
using MediatR;

namespace Basket.API.Retrieve;

public record GetCartQuery(string AccountName) : IQuery<GetCartResult>;

public record GetCartResult(ShoppingCart Cart)
{
    public static implicit operator GetCartResult(ShoppingCart cart) => new(cart);
}

public class RetrieveQueryHandler(ICartRepository repo) : IRequestHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
        => await repo.GetCartAsync(request.AccountName, cancellationToken);
}