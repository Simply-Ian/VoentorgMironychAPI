using Basket.API.Infrastructure;
using Basket.API.Models;
using Common.Interfaces;
using FluentValidation;

namespace Basket.API.Save;

/* Типы команды и результата в одном файле с обработчиком, потому что
 1. Я видел такое в примерах проектов по VSA
 2. По аналогии с файлом, где описан эндпойнт, чтобы не путать SaveCartResult и SaveCartResponse
 */

public record SaveCartResult(string AccountName);
public record SaveCartCommand(ShoppingCart Cart) : ICommand<SaveCartResult>;

internal class SaveCartCommandValidator : AbstractValidator<SaveCartCommand>
{
    public SaveCartCommandValidator()
    {
        RuleFor(x => x.Cart.AccountName).NotEmpty();
        RuleFor(x => x.Cart.Items)
            .NotEmpty()
            .WithMessage("Cannot store an empty cart")
            .ForEach(
                itemRule => itemRule
                .Must(item => item.UnitPrice >= 0)
                .WithMessage((_, item) => $"Item price must be non-negative. Got the value: {item.UnitPrice} for item " +
                                          $"{item.ItemTitle}")
                .Must(item => item.Quantity > 0)
                .WithMessage((_, item) => $"Item quantity must be positive. Got {item.Quantity} for item {item.ItemTitle}")
            );
    }
}

internal class SaveCommandHandler(ICartRepository cartRepo) : ICommandHandler<SaveCartCommand, SaveCartResult>
{
    public async Task<SaveCartResult> Handle(SaveCartCommand request, CancellationToken cancellationToken)
    {
        await cartRepo.StoreCartAsync(request.Cart, cancellationToken);
        return new SaveCartResult(request.Cart.AccountName);
    }
}