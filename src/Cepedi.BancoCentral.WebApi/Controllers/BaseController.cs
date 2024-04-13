using Cepedi.BancoCentral.Shareable.Enums;
using Cepedi.BancoCentral.Shareable.Exceptions;
using Cepedi.Shareable.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using OperationResult;

namespace Cepedi.BancoCentral.Api.Controllers;
public class BaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<ActionResult> SendCommand(IRequest<Result> request, int statusCode = 200)
        => await _mediator.Send(request) switch
        {
            (true, _) => StatusCode(statusCode),
            var (_, error) => HandleError(error!)
        };

    protected async Task<ActionResult> SendCommand<T>(IRequest<T> request, int statusCode = 200)
    {
        var response = await _mediator.Send(request);

        return StatusCode(statusCode, response);
    }

    protected async Task<ActionResult<T>> SendCommand<T>(IRequest<Result<T>> request, int statusCode = 200)
        => await _mediator.Send(request).ConfigureAwait(false) switch
        {
            (true, var result, _) => StatusCode(statusCode, result),
            var (_, res, error) => HandleError(error!)
        };

    protected ActionResult HandleError(Exception error) => error switch
    {
        RequestInvalidaException e => BadRequest(FormatErrorMessage(e.ResponseErro, e.Erros)),
        SemResultadosException e => NoContent(),
        _ => BadRequest(FormatErrorMessage(BancoCentralMensagemErrors.Generico))
    };

    private ResponseErro FormatErrorMessage(ResponseErro responseErro, IEnumerable<string>? errors = null)
    {
        if (errors != null)
        {
            responseErro.Descricao = $"{responseErro.Descricao} : {string.Join("; ", errors!)}";
        }

        return responseErro;
    }
}
