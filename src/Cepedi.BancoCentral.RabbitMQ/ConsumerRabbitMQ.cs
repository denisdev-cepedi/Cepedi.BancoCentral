using System.Text;
using System.Text.Json;
using Cepedi.BancoCentral.Compartilhado.Enums;
using Cepedi.BancoCentral.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cepedi.BancoCentral.Dominio.Servicos.RabbitMQ;
public abstract class RabbitMQConsumer<T> : IConsumerRabbitMQ<T>
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _password;
    private readonly string _user;
    private readonly string _routingKey;
    private readonly string _exchangeName;
    private CancellationToken _cancellationToken;

    protected IConnection _connection;
    protected IModel _channel;
    protected EventingBasicConsumer _consumer;
    //private AsyncEventingBasicConsumer _consumer;
    private string _consumerTag;

    protected RabbitMQConsumer(IConfiguration configuration)
    {
        _hostname = configuration["RabbitMQ:Hostname"]!;
        _queueName = configuration["RabbitMQ:QueueName"]!;
        _password = configuration["RabbitMQ:Password"]!;
        _user = configuration["RabbitMQ:User"]!;
        _routingKey = configuration["RabbitMQ:RoutingKey"]!;
        _exchangeName = configuration["RabbitMQ:Exchange"]!;
    }

    public Task IniciaLeituraMensagens(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _cancellationToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory() { HostName = _hostname, Port = 5672, UserName = _user, Password = _password };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        //_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey);



        //_consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += async (model, ea) =>
        {
            //var body = ea.Body.ToArray();
            //var message = Encoding.UTF8.GetString(body);
            var tentativa = QuantidadeRententativa(ea.BasicProperties);

            var shouldAcknowledge = await LeMensagemAsync(model, tentativa, ea);


            switch (shouldAcknowledge)
            {
                case ResultadoProcessamento.Success:
                    Ack(ea.DeliveryTag);
                    break;
                case ResultadoProcessamento.Error:
                    Nack(ea.DeliveryTag);
                    break;
                case ResultadoProcessamento.TryAgain:
                    {
                        NackRequeue(ea.DeliveryTag);
                        IncrementarRententaiva(ea.BasicProperties, tentativa + 1);
                    }
                    break;
            }

        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: _consumer);

        _cancellationToken.ThrowIfCancellationRequested();

        return Task.CompletedTask;
    }

    private async Task<ResultadoProcessamento> LeMensagemAsync(object sender, int tentativa, BasicDeliverEventArgs e)
    {
        try
        {
            var mensagem = JsonSerializer.Deserialize<T>(e.Body.ToArray());

            return await ProcessarMensagem(mensagem, tentativa, _cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ResultadoProcessamento.Error;
        }
    }

    private void NackRequeue(ulong deliveryTag)
    {
        if (deliveryTag != 0)
        {
            _channel.BasicNack(deliveryTag, multiple: false, requeue: true);
        }
    }

    private void Nack(ulong deliveryTag)
    {
        if (deliveryTag != 0)
        {
            _channel.BasicNack(deliveryTag, multiple: false, requeue: false);
        }
    }

    private void Ack(ulong deliveryTag)
    {
        if (deliveryTag != 0)
        {
            _channel.BasicAck(deliveryTag, false);
        }
    }

    public abstract Task<ResultadoProcessamento> ProcessarMensagem(T? mensagem, int tentativa, CancellationToken cancellationToken);

    private int QuantidadeRententativa(IBasicProperties basicProperties)
    {
        var headers = basicProperties.Headers;
        var count = 0;

        if (headers?.ContainsKey("retry.count") ?? false)
        {
            var countStr = Convert.ToString(headers["hr.retry.count"]);
            count = Convert.ToInt32(headers["count"]);
        }

        return count;
    }

    private void IncrementarRententaiva(IBasicProperties properties, int nackCount)
    {
        if (properties == null)
        {
            properties = _channel.CreateBasicProperties();
        }
        properties.Headers = properties.Headers ?? new Dictionary<string, object>();
        properties.Headers["retry.count"] = BitConverter.GetBytes(nackCount);
    }

    public void Finaliza()
    {
        if (_consumer != null && _consumer.IsRunning)
        {
            return;
        }

        //_consumer.Received -= LeMensagemAsync;

        _channel.Close();
        _channel?.Dispose();
    }
}
