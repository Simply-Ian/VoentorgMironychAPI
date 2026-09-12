using Catalog.Application.Queries.ItemQueries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using Catalog.Domain.Specifications;
using FluentValidation;

namespace Catalog.Application.Validators;

/// <warning>
/// Регистрировать как transient или следить за тем, чтобы в одном запросе использовался лишь однажды,
/// т. к. имеет состояние
/// </warning>
public class GetItemsWithPaginationQueryValidator : AbstractValidator<GetItemsWithPaginationQuery>
{
    private int pagesCount;
    public GetItemsWithPaginationQueryValidator(IRepository repo)
    {
        RuleFor(query => query.Args.PageIndex).GreaterThanOrEqualTo(0);
        RuleFor(query => query.Args.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(query => query.Args.PageIndex)
            .MustAsync
            (async (query, index, _) =>
            {
                pagesCount = PaginatedResponse<Item>.CalculatePagesCount(await repo.CountAsync<Item>(), query.Args.PageSize);
                return index < pagesCount;
            })
            // lambda, чтобы строка формировалась после проверки MustAsync и записи в pagesCount
            .WithMessage(query => $"Page index must not exceed <total pages count> - 1. There are {pagesCount} pages in total.")
            .When(query => query.Args.PageSize >= 1);

        RuleFor(query => query.Args.MinPrice)
            .LessThanOrEqualTo(query => query.Args.MaxPrice)
            .When(query => query.Args is { MinPrice: not null, MaxPrice: not null })
            .WithMessage(query => $"MinPrice must be less or equal to MaxPrice. " +
                                  $"Given: MinPrice = {query.Args.MinPrice}, MaxPrice = {query.Args.MaxPrice}.");
        
        RuleFor(query => query.Args.FieldToSortBy).Must(key => string.IsNullOrEmpty(key) 
                                                               || key.ToLower() == nameof(Item.Price).ToLower() 
                                                               || key.ToLower() == nameof(Item.Title).ToLower());
    }
}