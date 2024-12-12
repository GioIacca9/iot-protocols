using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cloud;
public class AMQPProvider : IDisposable
{
    private readonly IConfiguration _configuration;
    private ConnectionFactory? factory;
    private AsyncEventingBasicConsumer? consumer;
    private IConnection? connection;
    private IChannel? channel;
    private bool disposedValue;
    public event EventHandler<BasicDeliverEventArgs>? MessageReceived;

    public AMQPProvider()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        factory = new ConnectionFactory
        {
            HostName = _configuration["AMQP:Hostname"],
            VirtualHost = _configuration["AMQP:VirtualHost"],
            UserName = _configuration["AMQP:Username"],
            Password = _configuration["AMQP:Password"]
        };
    }

    public async Task Start()
    {
        if (factory != null)
        {
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "states", durable: false, exclusive: false, autoDelete: false,
                arguments: null, noWait: true);
            consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += CunsumerReceived;
            await channel.BasicQosAsync(0, 10, true);
            await channel.BasicConsumeAsync("states", autoAck: true, consumer: consumer);
        }
        else throw new Exception("Cannot create connection factory for AMQP");
    }

    private Task CunsumerReceived(object sender, BasicDeliverEventArgs @event)
    {
        MessageReceived?.Invoke(sender, @event);
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                channel?.CloseAsync();
                connection?.CloseAsync();
                factory = null;
                channel = null;
                connection = null;
            }
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
