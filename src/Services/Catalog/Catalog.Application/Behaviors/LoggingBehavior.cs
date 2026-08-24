using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> 
(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
): IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Started handling request {request}");
        var result = await next.Invoke(cancellationToken);
        logger.LogInformation($"Finished handling request {request}");
        return result;
    }
}