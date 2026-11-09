using Common.Interfaces;
using FluentValidation;
using Mapster;
using Promotion.Grpc.Models;
using Promotion.Grpc.Persistence;

namespace Promotion.Grpc.Features;

public record StorePromoCommand(PromoDto Dto) : ICommand<StoreCommandResult>;

public record StoreCommandResult(bool InstanceAlreadyExists);

public class StorePromoCommandHandler(
    IPromoRepository repo, 
    IValidator<Promo> promoValidator
) : ICommandHandler<StorePromoCommand, StoreCommandResult>
{
    /// <returns>
    /// true, если запись с указанным Id уже существовала в БД (т. е. было выполнено обновление), иначе false
    /// </returns>
    public async Task<StoreCommandResult> Handle(StorePromoCommand request, CancellationToken cancellationToken)
    {
        var promoInstance = request.Dto.Adapt<Promo>();
        promoValidator.ValidateAndThrow(promoInstance);
        var alreadyExists = await repo.StoreAsync(promoInstance);
        return new (alreadyExists);
    }
}

public class PromoValidator : AbstractValidator<Promo>
{
    public PromoValidator()
    {
        /* Id не валидируется, при маппинге dto для некорректного Id будет выброшено исключение, которое потом поймает
        глобальный обработчик */
        RuleFor(promo => promo.Value).GreaterThan(0);
    }
}