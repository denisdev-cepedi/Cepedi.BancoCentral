using Cepedi.Shareable.Exceptions;
using Cepedi.Shareable.Requests;
using Cepedi.Shareable.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cepedi.BancoCentral.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;

    public UserController(
        ILogger<UserController> logger, IMediator mediator)
        : base(mediator)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CriarUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErro), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CriarUsuarioResponse>> CriarUsuarioAsync(
        [FromBody] CriarUsuarioRequest request) => await EnviarCommand(request);

    [HttpPut]
    [ProducesResponseType(typeof(AtualizarUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErro), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErro), StatusCodes.Status204NoContent)]
    public async Task<ActionResult<AtualizarUsuarioResponse>> AtualizarUsuarioAsync(
        [FromBody] AtualizarUsuarioRequest request) => await EnviarCommand(request);

}
