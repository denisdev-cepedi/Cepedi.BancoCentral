using Cepedi.BancoCentral.Domain.Entities;
using Cepedi.BancoCentral.Domain.Repository;
using Cepedi.BancoCentral.Shareable.Enums;
using Cepedi.Shareable.Requests;
using Cepedi.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace Cepedi.BancoCentral.Domain.Handlers;
public class CriarUsuarioRequestHandler : IRequestHandler<CriarUsuarioRequest, Result<CriarUsuarioResponse>>
{
    private readonly ILogger<CriarUsuarioRequestHandler> _logger;
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioRequestHandler(IUsuarioRepository usuarioRepository, ILogger<CriarUsuarioRequestHandler> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<Result<CriarUsuarioResponse>> Handle(CriarUsuarioRequest request, CancellationToken cancellationToken)
    {
        try
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

            return Result.Success(new CriarUsuarioResponse(usuario.Id, usuario.Nome));
        }
        catch
        {
            _logger.LogError("Ocorreu um erro durante a execução");
            return Result.Error<CriarUsuarioResponse>(new Shareable.Exceptions.ApplicationException(
                (BancoCentralMensagemErrors.ErroGravacaoUsuario)));
        }
    }
}
