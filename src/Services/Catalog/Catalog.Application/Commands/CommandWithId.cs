using Catalog.Domain.Entities;

namespace Catalog.Application.Commands;

/// <typeparam name="TEntity">Нужен для валидации</typeparam>
public abstract record CommandWithId<TEntity>(Guid Id) where TEntity : BaseEntity;