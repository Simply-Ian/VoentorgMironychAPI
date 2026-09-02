using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public static class ExtensionMethods
{
    public static IRuleBuilderOptions<T, Guid?> EntityMustExist<T, TEntity>
    (
        this IRuleBuilder<T, Guid?> ruleBuilder,
        IRepository repo
    ) where TEntity : BaseEntity => ruleBuilder.MustAsync(
        async (id, _) => await repo.GetByIdAsync<TEntity>(id!.Value) != null
    );
    
    public static IRuleBuilderOptions<T, Guid> EntityMustExistNonNullable<T, TEntity>
    (
        this IRuleBuilder<T, Guid> ruleBuilder,
        IRepository repo
    ) where TEntity : BaseEntity => ruleBuilder.MustAsync(
        async (id, _) => await repo.GetByIdAsync<TEntity>(id) != null
    );
}