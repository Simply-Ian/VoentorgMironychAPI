using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Application.Handlers;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public class UpdateItemCommandValidator : AbstractValidator<UpdateEntityCommand<CreateItemContract, Item>> 
{
    /* Stateless. Not injected via DI because it only represents a part of functionality shared between
    DeleteEntityCommandValidator and UpdateEntityCommandValidator. It completely relies on IRepository method calls,
    and thus it is no different from an interface method call wrapped into a class. */
    private readonly ExistenceChecker _existenceChecker = new();
    
    public UpdateItemCommandValidator(IRepository repo, IValidator<CreateItemContract> contractValidator)
    {
        RuleFor(command => command.Contract)
            .MustAsync(async (contract, _) =>
            {
                await contractValidator.ValidateAndThrowAsync(contract);
                return true;
            });
        RuleFor(command => command.Id)
            .MustAsync(async (id, _) => await _existenceChecker.AssureEntityExists<Item>(id, repo))
            .WithMessage($"Item with Id = {{PropertyValue}} not found");
    }
}