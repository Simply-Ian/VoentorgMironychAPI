using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Domain.Entities;
using FluentValidation;

namespace Catalog.Application.Validators;

public class CreateItemCommandValidator : AbstractValidator<CreateEntityCommand<CreateItemContract, Item>>
{
    public CreateItemCommandValidator(IValidator<CreateItemContract> validator)
    {
        RuleFor(command => command)
            .MustAsync(async (command, _) =>
            {
                await validator.ValidateAndThrowAsync(command.Contract);
                return true;
            });
    }
}