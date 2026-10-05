using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Catalog.Application.Validators;

public class CommandWithIdValidator<TEntity> : AbstractValidator<CommandWithId<TEntity>>
where TEntity: BaseEntity
{
    public CommandWithIdValidator(IRepository repo)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, _) => await repo.GetByIdAsync<TEntity>(id) != null)
            .WithMessage($"{typeof(TEntity).Name} with Id = {{PropertyValue}} not found");
    }
    
    protected override void RaiseValidationException(
        ValidationContext<CommandWithId<TEntity>> context, 
        ValidationResult result
    ) => throw new NotFoundException(result.Errors);
}