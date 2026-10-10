using Basket.API.Infrastructure;
using Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Basket.API.Delete;

internal record DeleteCartCommand(string AccountName) : ICommand;

/// <summary>
/// Не проверяет существование корзины с таким именем: если корзина не существует, репозиторий выбросит NotFoundException
/// </summary>
internal class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartCommandValidator()
    {
        RuleFor(x => x.AccountName).NotEmpty().WithMessage("The account name must be non-empty");
    }
}

internal class DeleteCommandHandler(ICartRepository repo) : ICommandHandler<DeleteCartCommand>
{
    public async Task<Unit> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        await repo.RemoveCartAsync(request.AccountName, cancellationToken);
        return Unit.Value;
    }
}