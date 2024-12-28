using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<RabbitMqOptions>(
            hostContext.Configuration.GetSection("RabbitMq"));
        
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();