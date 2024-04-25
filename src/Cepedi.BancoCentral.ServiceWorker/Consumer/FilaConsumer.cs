using Cepedi.BancoCentral.Compartilhado.Enums;
using Cepedi.BancoCentral.Dominio.Servicos.RabbitMQ;
using Cepedi.Shareable.Requests;
using MediatR;

namespace Cepedi.BancoCentral.ServiceWorker.Consumer;
public class FilaConsumer : RabbitMQConsumer<CriarUsuarioRequest>
{
    private readonly IServiceProvider _serviceProvider;

    public FilaConsumer(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
        : base(configuration)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<ResultadoProcessamento> ProcessarMensagem(CriarUsuarioRequest mensagem, int tentativa, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            cancellationToken.ThrowIfCancellationRequested();

            var (sucesso, resultado, erro) = await mediator.Send(mensagem, cancellationToken);

            if (erro != null)
            {
                // incluir log
            }

            return sucesso switch
            {
                true => ResultadoProcessamento.Success,
                false => tentativa < 3 ? ResultadoProcessamento.TryAgain : ResultadoProcessamento.Error,
            };
        }
        catch (Exception ex)
        {

            return ResultadoProcessamento.Error;
        }
    }
}
