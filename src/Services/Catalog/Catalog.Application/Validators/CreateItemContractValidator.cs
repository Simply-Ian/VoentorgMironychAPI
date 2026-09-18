using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

/// <summary>
/// Валидатор контракта вынесен в отдельный класс, несмотря на то, что валидатор всей команды ничего больше не проверяет,
/// чтобы логику можно было переиспользовать в валидаторе команды на обновление
/// </summary>
public class CreateItemContractValidator : AbstractValidator<CreateItemContract>
{
    public CreateItemContractValidator(IRepository repo)
    {
        RuleFor(contract => contract.Price).GreaterThan(0);
        
        RuleFor(contract => contract.ImageUrl)
            .Must(url=> url?.IsWellFormedOriginalString() ?? true)
            .WithMessage("{PropertyName} must be valid");
        
        RuleFor(contract => contract.BrandId)
            .EntityMustExist<CreateItemContract, Brand>(repo)
            .WithMessage("Unknown {PropertyName} is specified")
            .When(command => command.BrandId != null);
        
        RuleForEach(contract => contract.CategoryIds)
            .EntityMustExistNonNullable<CreateItemContract, Category>(repo)
            .WithMessage("Unknown category ID is specified at {CollectionIndex}")
            .When(contract => contract.CategoryIds != null);
    }
}