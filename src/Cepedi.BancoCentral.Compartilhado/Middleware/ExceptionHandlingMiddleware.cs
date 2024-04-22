using System.Text.Json;
using Cepedi.BancoCentral.Compartilhado.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cepedi.BancoCentral.Compartilhado.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            Titulo = GetTitle(exception),
            status = statusCode,
            Mensagem = exception.Message,
            errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            RequestInvalidaExcecao => StatusCodes.Status422UnprocessableEntity,
            SemResultadosExcecao => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status500InternalServerError
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            Compartilhado.Exceptions.ExcecaoAplicacao applicationException => applicationException.ResponseErro.Titulo,
            _ => "Server Error"
        };
    private static IEnumerable<string> GetErrors(Exception exception)
    {
        IEnumerable<string> errors = null;
        if (exception is RequestInvalidaExcecao validationException)
        {
            errors = validationException!.Erros;
        }
        return errors;
    }
}
