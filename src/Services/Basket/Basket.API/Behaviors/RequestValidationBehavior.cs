using FluentValidation;
using MediatR;

namespace Basket.API.Behaviors;

/* Кстати: IPipelineBehavior контравариантен по типу запроса, т. е. IPipelineBehavior<BaseRequest> можно привести к
IPipelineBehavior<ConcreteRequest>. Предположу, что это нужно для корректного разрешения зависимостей в DI: при
запросе у контейнера поведения для конкретного request-а вернутся в том числе и поведения для базовых типов этого request-а? */
// ServiceProvider, т. к. не для всех запросов определены валидаторы
public class RequestValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse: notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errors = (await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(request, cancellationToken))
            ))
            .SelectMany(v => v.Errors)
            .ToList();
        if (errors.Any()) throw new ValidationException(errors);
        return await next.Invoke(cancellationToken);
    }
}