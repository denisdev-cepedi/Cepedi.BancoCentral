using Cepedi.BancoCentral.RabbitMQ;
using Cepedi.BancoCentral.ServiceWorker.Consumer;
using Cepedi.Shareable.Requests;

namespace Cepedi.BancoCentral.ServiceWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumerRabbitMQ<CriarUsuarioRequest> _criaUsuarioConsumer;

    public Worker(ILogger<Worker> logger,
        IConsumerRabbitMQ<CriarUsuarioRequest> criaUsuarioConsumer)
    {
        _logger = logger;
        _criaUsuarioConsumer = criaUsuarioConsumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            stoppingToken.Register(Finaliza);

            return Inicia(stoppingToken);
        }
        catch(OperationCanceledException ex)
        {
            return Task.FromCanceled(stoppingToken);
        }
        catch(Exception ex)
        {
            _logger.LogError("OCorreu um erro");
            return Task.FromException(ex);
        }
      
    }

    private Task Inicia(CancellationToken stoppingToken)
    {
        return Task.WhenAll(_criaUsuarioConsumer.IniciaLeituraMensagens(stoppingToken));
    }

    private void Finaliza()
    {
        _criaUsuarioConsumer.Finaliza();
    }
}
