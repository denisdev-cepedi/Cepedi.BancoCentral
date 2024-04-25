using System.Collections.Concurrent;
using System.Text;
using Cepedi.BancoCentral.Dominio.Servicos.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Cepedi.BancoCentral.RabbitMQ;

public class ProducerRabbitMQ : IProducerRabbitMQ
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _password;
    private readonly string _user;

    public ProducerRabbitMQ(IConfiguration configuration)
    {
        _hostname = configuration["RabbitMQ:Hostname"]!;
        _queueName = configuration["RabbitMQ:QueueName"]!;
        _password = configuration["RabbitMQ:Password"]!;
        _user = configuration["RabbitMQ:User"]!;
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostname, Port = 5672, UserName = _user, Password = _password };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            IBasicProperties retry = channel.CreateBasicProperties();
            retry.Headers = retry.Headers ?? new Dictionary<string, object>();
            retry.Headers["retry.count"] = 0;

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
