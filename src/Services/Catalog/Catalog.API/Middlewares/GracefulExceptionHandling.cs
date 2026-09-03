using System.Text.Json;
using FluentValidation;

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
            if (!await HandleIt(exc, context)) throw;
        }
    }

    /// <summary>
    /// Обработчик намеренно не включает в себя случай "по умолчанию", т. к. я хочу сохранить преднастроенное
    /// поведение, при котором необработанное исключение выбрасывает 500
    /// </summary>
    private async Task<bool> HandleIt(Exception exception, HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var result = string.Empty;
        bool isHandled = false;
        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                isHandled = true;
                break;
        }

        if (isHandled) await context.Response.WriteAsync(result);
        return isHandled;
    }
}