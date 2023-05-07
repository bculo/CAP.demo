using Amazon;
using Amazon.Runtime;
using CAP.Application.Consumers;
using Savorboard.CAP.InMemoryMessageQueue;

namespace CAP.API;

public static class ApiConfiguration
{
    public static void ConfigureTransportLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var transportType = (CapTransportType)configuration.GetValue<int>("CapOptions:TransportType");
        
        switch (transportType)
        {
            case CapTransportType.InMemory:
                services.ConfigureInMemoryTransport(configuration);
                break;
            case CapTransportType.RabbitMQ:
                services.ConfigureRabbitMqTransport(configuration);
                break;
            case CapTransportType.Kafka:
                services.ConfigureKafkaTransport(configuration);
                break;
            case CapTransportType.AmazonSQS:
                services.ConfigureAmazonSQSTransport(configuration);
                break;
            default:
                throw new NotSupportedException("Message broker not supported");
        }

        services.AddTransient<OrderReceivedConsumer>();
    }

    private static void ConfigureInMemoryTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(x =>
        {
            x.UseInMemoryStorage();
            x.UseInMemoryMessageQueue();
            x.UseDashboard();
        });
    }
    
    private static void ConfigureRabbitMqTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(x =>
        {
            x.UseRabbitMQ(opt=>
            {
                opt.HostName = "localhost";
                opt.Port = 5672;
                opt.Password = "pass";
                opt.UserName = "user";
                opt.ExchangeName = "CapDemoExchange";
            });
            
            x.UseInMemoryStorage();
            x.UseDashboard();
        });
    }

    private static void ConfigureKafkaTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(x =>
        {
            x.UseKafka("localhost:9092");
            x.UseInMemoryStorage();
            x.UseDashboard();
        });
    }
    
    private static void ConfigureAmazonSQSTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(x =>
        {
            x.UseAmazonSQS(options =>
            {
                options.SNSServiceUrl = "http://localhost:4566/";
                options.SQSServiceUrl = "http://localhost:4566/";
            });
            
            x.UseInMemoryStorage();
            x.UseDashboard();
        });
    }
}

public enum CapTransportType
{
    InMemory = 0,
    RabbitMQ = 1,
    Kafka = 2,
    AmazonSQS = 3
}