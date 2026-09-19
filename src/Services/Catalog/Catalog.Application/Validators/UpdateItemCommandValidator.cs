using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Application.Handlers;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public class UpdateItemCommandValidator : AbstractValidator<UpdateEntityCommand<CreateItemContract, Item>> 
{
    public UpdateItemCommandValidator(
        IValidator<CreateItemContract> contractValidator, 
        IValidator<CommandWithId<Item>> commandValidator
    )
    {
        Include(commandValidator);
        RuleFor(command => command.Contract)
            .MustAsync(async (contract, _) =>
            {
                await contractValidator.ValidateAndThrowAsync(contract);
                return true;
            });
    }
}