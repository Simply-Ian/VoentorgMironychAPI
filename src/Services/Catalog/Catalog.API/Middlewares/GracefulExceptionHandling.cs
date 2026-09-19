using System.Text.Json;
using FluentValidation;
using Catalog.Application.Validators; // NotFoundException

namespace Catalog.API.Middlewares;

/// <summary>
/// Обрабатывает типичные исключения, возникшие при обработке запросов. Отличия от поведения MediatR:
/// выставляет статус-коды и сообщения об ошибках в ответе.
/// Я предпочел паттерну Result обработку исключений через middleware, поскольку хочу централизованно описать типичные
/// исключения, такие как ValidationException, и ответы на них, вместо того, чтобы из раза в раз прописывать
/// try-catch-400 в action-ах контроллеров
/// </summary>
public class GracefulExceptionHandling(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exc)
        {
            await HandleIt(exc, context);
        }
    }

    /// <summary>
    /// Обработчик намеренно не включает в себя случай "по умолчанию", т. к. я хочу сохранить преднастроенное
    /// поведение, при котором необработанное исключение выбрасывает 500
    /// </summary>
    private static async Task HandleIt(Exception exception, HttpContext context)
    {
        context.Response.ContentType = "application/json";
        string result;
        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
            
            case NotFoundException notFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                result = JsonSerializer.Serialize(notFoundException.Errors);
                break;
            
            default: throw exception;
        }
        await context.Response.WriteAsync(result);
    }
}