using Cepedi.BancoCentral.Compartilhado.Enums;
using Cepedi.BancoCentral.Compartilhado.Exceptions;
using Cepedi.BancoCentral.Compartilhado.Excecoes;
using MediatR;
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

    protected async Task<ActionResult> EnviarCommand(IRequest<Result> request, int statusCode = 200)
        => await _mediator.Send(request) switch
        {
            (true, _) => StatusCode(statusCode),
            var (_, error) => ManipularErro(error!)
        };

    protected async Task<ActionResult> EnviarCommand<T>(IRequest<T> request, int statusCode = 200)
    {
        var response = await _mediator.Send(request);

        return StatusCode(statusCode, response);
    }

    protected async Task<ActionResult<T>> EnviarCommand<T>(IRequest<Result<T>> request, int statusCode = 200)
        => await _mediator.Send(request).ConfigureAwait(false) switch
        {
            (true, var result, _) => StatusCode(statusCode, result),
            var (_, res, error) => ManipularErro(error!)
        };

    protected ActionResult ManipularErro(Exception error) => error switch
    {
        RequestInvalidaExcecao e => BadRequest(FormatarMensagem(e.ResponseErro, e.Erros)),
        SemResultadosExcecao e => NoContent(),
        _ => BadRequest(FormatarMensagem(BancoCentralMensagemErrors.Generico))
    };

    private ResultadoErro FormatarMensagem(ResultadoErro responseErro, IEnumerable<string>? errors = null)
    {
        if (errors != null)
        {
            responseErro.Descricao = $"{responseErro.Descricao} : {string.Join("; ", errors!)}";
        }

        return responseErro;
    }
}
