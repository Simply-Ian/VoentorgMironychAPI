using System.Collections;
using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc; // ProblemDetails, сборка в действительности не подтягивается

namespace Common.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _setHeaders(context.Response, exception);
        var problemDetails = _createProblemDetails(context, exception);
        await _responseAndLog(context, exception, problemDetails, cancellationToken);

        return true;
    }
    private static void _setHeaders(in HttpResponse response, in Exception exception)
    {
        response.ContentType = "application/json";
        response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    private static ProblemDetails _createProblemDetails(in HttpContext context, in Exception exception)
    {
        IEnumerable? errorsList = null;
        var errorsProperty = exception.GetType().GetProperty("Errors");
        if (errorsProperty is not null)
            errorsList = (IEnumerable?)errorsProperty.GetValue(exception);
        var problemDetailsExtensions = new Dictionary<string, object?>
        {
            ["TraceId"] = context.TraceIdentifier,
            ["Errors"] = errorsList
        };
        return new ProblemDetails
        {
            Type = exception.GetType().Name,
            Detail = exception.Message,
            Instance = context.Request.Path.ToString(),
            Status = context.Response.StatusCode,
            Extensions = problemDetailsExtensions
        };
    }
    private async ValueTask _responseAndLog(HttpContext context, Exception exception, ProblemDetails problemDetails,
        CancellationToken cancellationToken)
    {
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        logger.LogError(
            exception, 
            $"An error occured when handling request {context.TraceIdentifier} to {context.Request.Path}"
        );
    }
}