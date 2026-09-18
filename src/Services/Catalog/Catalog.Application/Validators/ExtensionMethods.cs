using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public static class ExtensionMethods
{

    /// <typeparam name="T">Нужен, чтобы компилятор не ругался при сборке цепочки из правил</typeparam>
    public static IRuleBuilderOptions<T, Guid?> EntityMustExist<T, TEntity>
    (
        this IRuleBuilder<T, Guid?> ruleBuilder,
        IRepository repo
    ) where TEntity : BaseEntity => ruleBuilder.MustAsync(
        async (id, _) => await repo.GetByIdAsync<TEntity>(id!.Value) != null
    );
    
    /// <typeparam name="T">Нужен, чтобы компилятор не ругался при сборке цепочки из правил</typeparam>
    public static IRuleBuilderOptions<T, Guid> EntityMustExistNonNullable<T, TEntity>
    (
        this IRuleBuilder<T, Guid> ruleBuilder,
        IRepository repo
    ) where TEntity : BaseEntity => ruleBuilder.MustAsync(
        async (id, _) => await repo.GetByIdAsync<TEntity>(id) != null
    );
}