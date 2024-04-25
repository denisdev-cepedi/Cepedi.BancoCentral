using Cepedi.BancoCentral.Dominio.Entidades;
using Cepedi.BancoCentral.Dominio.Repositorio;
using Cepedi.BancoCentral.Dominio.Servicos.RabbitMQ;
using Cepedi.Shareable.Requests;
using Cepedi.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace Cepedi.BancoCentral.Dominio.Handlers;
public class CriarUsuarioRequestHandler
    : IRequestHandler<CriarUsuarioRequest, Result<CriarUsuarioResponse>>
{
    private readonly ILogger<CriarUsuarioRequestHandler> _logger;
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioRequestHandler(
        IUsuarioRepository usuarioRepository,
        ILogger<CriarUsuarioRequestHandler> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<Result<CriarUsuarioResponse>> Handle(CriarUsuarioRequest request, CancellationToken cancellationToken)
    {

        var usuario = new UsuarioEntity()
        {
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            Celular = request.Celular,
            CelularValidado = request.CelularValidado,
            Email = request.Email,
            Cpf = request.Cpf
        };

        await _usuarioRepository.CriarUsuarioAsync(usuario);

        return new CriarUsuarioResponse(usuario.Id, usuario.Nome);
    }
}
