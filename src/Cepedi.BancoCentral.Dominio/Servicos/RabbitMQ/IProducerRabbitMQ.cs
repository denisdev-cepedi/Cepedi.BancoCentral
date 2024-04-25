namespace Cepedi.BancoCentral.Dominio.Servicos.RabbitMQ;
public interface IProducerRabbitMQ
{
    void SendMessage(string message);
}
