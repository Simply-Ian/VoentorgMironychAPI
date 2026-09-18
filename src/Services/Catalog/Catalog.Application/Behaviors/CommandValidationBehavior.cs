using FluentValidation;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Behaviors;

// Регать с явным указанием типов. MediatR регает поведения как transient
public class CommandValidationBehavior<TRequest, TResponse>
(
    IValidator<TRequest> validator,
    IHostEnvironment environment,
    ILogger<CommandValidationBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
where TRequest: IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
        }
        catch (ValidationException exc)
        {
            if (environment.IsDevelopment()) 
                logger.LogWarning
                (
                    "Attempted to handle an invalid command. The following errors were found during validation: {Errors}",
                    exc.Errors
                );
            throw;
        }

        return await next.Invoke(cancellationToken);
    }
}