using Cepedi.BancoCentral.IoC;
using Cepedi.BancoCentral.RabbitMQ;
using Cepedi.BancoCentral.ServiceWorker;
using Cepedi.BancoCentral.ServiceWorker.Consumer;
using Cepedi.Shareable.Requests;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.ConfigureAppDependencies(hostContext.Configuration);
        services.AddSingleton<IConsumerRabbitMQ<CriarUsuarioRequest>, FilaConsumer>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
