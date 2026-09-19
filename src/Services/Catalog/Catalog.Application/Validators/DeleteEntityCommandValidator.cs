using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using FluentValidation;

namespace Catalog.Application.Validators;

public class DeleteEntityCommandValidator<TEntity> : AbstractValidator<DeleteEntityCommand<TEntity>>
    where TEntity: BaseEntity
{
    public DeleteEntityCommandValidator(IValidator<CommandWithId<TEntity>> validator)
    {
        Include(validator);
    }
}