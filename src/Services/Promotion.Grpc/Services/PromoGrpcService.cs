using FluentValidation;
using Grpc.Core;
using Mapster;
using MediatR;
using Promotion.Grpc.Features;

namespace Promotion.Grpc.Services;

public class PromoGrpcService(ISender mediator) : PromoService.PromoServiceBase
{
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

    public override async Task<Empty> StorePromo(PromoDto request, ServerCallContext context)
    {
        StorePromoCommand command = new (request);
        var operationResult = await mediator.Send(command);
        context.Status = new(
            operationResult.InstanceAlreadyExists? StatusCode.AlreadyExists : StatusCode.OK, 
            "");
        return new();
    }
}
        