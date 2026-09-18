using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public class DeleteEntityCommandValidator<TEntity> : AbstractValidator<DeleteEntityCommand<TEntity>>
    where TEntity: BaseEntity
{
    /* Stateless. Not injected via DI because it only represents a part of functionality shared between
    DeleteEntityCommandValidator and UpdateEntityCommandValidator. It completely relies on IRepository method calls,
    and thus it is no different from an interface method call wrapped into a class. */
    private readonly ExistenceChecker _existenceChecker = new();
    
    public DeleteEntityCommandValidator(IRepository repo)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, _) => await _existenceChecker.AssureEntityExists<Item>(id, repo))
            .WithMessage($"{typeof(TEntity).Name} with Id = {{PropertyValue}} not found");
    }
}