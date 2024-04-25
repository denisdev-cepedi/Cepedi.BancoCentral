namespace Cepedi.BancoCentral.RabbitMQ;
public interface IConsumerRabbitMQ<T>
{
    Task IniciaLeituraMensagens(CancellationToken cancellationToken);
    void Finaliza();
}
