using FluentValidation;
using MediatR;

namespace Basket.API.Behaviors;

// ServiceProvider, т. к. не для всех запросов определены валидаторы
public class RequestValidationBehavior<TRequest, TResponse>(IServiceProvider services) : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse: notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validator = services.GetService<IValidator<TRequest>>();
        if (validator is not null)
            await validator.ValidateAndThrowAsync(request, cancellationToken);
        return await next.Invoke(cancellationToken);
    }
}