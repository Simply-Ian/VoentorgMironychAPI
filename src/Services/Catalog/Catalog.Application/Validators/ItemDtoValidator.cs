using Catalog.Application.DTO;
using Catalog.Domain.Entities;
using Catalog.Domain.Repos;
using FluentValidation;

namespace Catalog.Application.Validators;

public class ItemDtoValidator : AbstractValidator<ItemDTO>
{
    public ItemDtoValidator(IRepository repo)
    {
        RuleFor(item => item.Price).GreaterThan(0);
        
        RuleFor(item => item.ImageUrl)
            .Must(url=> url?.IsWellFormedOriginalString() ?? true)
            .WithMessage("{PropertyName} must be valid");
        
        RuleFor(item => item.BrandId)
            .EntityMustExist<ItemDTO, Brand>(repo)
            .WithMessage("Unknown {PropertyName} is specified")
            .When(dto => dto.BrandId != null);
        
        RuleForEach(item => item.CategoryIds)
            .EntityMustExistNonNullable<ItemDTO, Category>(repo)
            .WithMessage("Unknown category ID is specified at {CollectionIndex}")
            .When(item => item.CategoryIds != null);
    }
}