using FluentValidation;
using Grpc.Core;
using Mapster;
using MediatR;
using Promotion.Grpc.Features;

namespace Promotion.Grpc.Services;

public class PromoGrpcService(ISender mediator) : PromoService.PromoServiceBase
{
    // TODO: как документировать?
    public override async Task<PromoDto> GetPromo(GetPromoRequest request, ServerCallContext context)
    {
        GetPromoQuery query;
        try
        {
            query = request.Adapt<GetPromoQuery>();
        }
        catch (FormatException exc)
        {
            throw new ValidationException(
                $"Validation exception occured", 
                [
                    new (
                        nameof(request.CatalogItemId), 
                        $"Error when parsing provided Guid: {exc.Message}",
                        request.CatalogItemId
                    )
                ]
            );
        }
        
        var result = await mediator.Send(query);
        return result.Adapt<PromoDto>();
    }
}
        