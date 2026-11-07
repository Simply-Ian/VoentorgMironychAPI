using Common.Interfaces;
using MediatR;
using Promotion.Grpc.Persistence;

namespace Promotion.Grpc.Features;

public record DeletePromoCommand(Guid CatalogItemId) : ICommand;

public class DeletePromoCommandHandler(IPromoRepository repo) : ICommandHandler<DeletePromoCommand>
{
    public async Task<Unit> Handle(DeletePromoCommand request, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(request.CatalogItemId);
        return Unit.Value;
    }
}