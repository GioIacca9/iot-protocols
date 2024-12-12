using MQTTnet.Client;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NetCoreClient.Protocols;

public class AMQPProvider : IAsyncDisposable
{
    private ConnectionFactory _connectionFactory;
    private bool _disposed;
    private IChannel? _channel;
    private IConnection? _connection;
    public AMQPProvider(string hostname, string username, string password, int port)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password,
            Port = port,
            VirtualHost = "/",
        };
    }

    public async Task<bool> ConnectAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        return _channel.IsOpen;
    }

    public async Task<Task> DeclareQueueAsync(string queue)
    {
        if (_channel is not null)
        {
            await _channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        return Task.CompletedTask;
    }

    public async Task PublishAsync(string message, string routingKey)
    {
        var body = Encoding.UTF8.GetBytes(message);

        if (_channel is not null)
        {
            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: routingKey, body: body);
            Debug.WriteLine($" [x] Sent {message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            if (disposing && _channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            _disposed = true;
        }
    }
}
