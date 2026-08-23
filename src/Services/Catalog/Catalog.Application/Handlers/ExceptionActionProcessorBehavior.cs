using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class ExceptionActionProcessorBehavior<TRequest, TResponse>
(
    ILogger<ExceptionActionProcessorBehavior<TRequest, TResponse>> logger
) : IRequestExceptionAction<TRequest, Exception>, IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task Execute(TRequest request, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError
        (
            exception, 
            $"{exception.GetType().FullName} was thrown while handling request {request}: {exception.Message}"
        );
        return Task.CompletedTask;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await next.Invoke(cancellationToken);
    }
}