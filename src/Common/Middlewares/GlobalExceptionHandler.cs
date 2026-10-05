using System.Collections;
using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        context.Response.ContentType = "application/json";
        IEnumerable? errorsList = null;
        var errorsProperty = exception.GetType().GetProperty("Errors");
        if (errorsProperty is not null)
            errorsList = (IEnumerable?)errorsProperty.GetValue(exception);
        context.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        logger.LogError(
            exception, 
            $"An error occured when handling request {context.TraceIdentifier} to {context.Request.Path}"
        );
        var problemDetailsExtensions = new
        {
            TraceId = context.TraceIdentifier,
            Errors = errorsList
        };
        var problemDetails = new
        {
            Type = exception.GetType().Name,
            Detail = exception.Message,
            Instance = context.Request.Path.ToString(),
            Status = context.Response.StatusCode,
            Extensions = problemDetailsExtensions
        };
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}