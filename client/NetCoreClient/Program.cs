using Microsoft.Extensions.Configuration;
using MQTTnet.Client;
using NetCoreClient.Protocols;
using NetCoreClient.Sensors;
using RabbitMQ.Client;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static IConfigurationRoot? _configuration;
    private static void Main()
    {
        ClientServices cloudServices = new();
        _configuration = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();
        _ = cloudServices.Start(
           _configuration["AMQP:hostname"] ?? "",
           _configuration["AMQP:username"] ?? "",
           _configuration["AMQP:password"] ?? "",
           int.Parse(_configuration["AMQP:port"] ?? "0")
           );

        while (true) { }
    }
}

public class ClientServices()
{
    public AMQPProvider? AMQPProvider { get; private set; }
    public async Task Start(string hostname, string username, string password, int port)
    {
        AMQPProvider = new(hostname, username, password, port);
        Dht11 sensor = new();
        if (AMQPProvider != null && await AMQPProvider.ConnectAsync())
        {
            //await AMQPProvider.DeclareQueueAsync("states");
            while (true)
            {
                sensor.ReadValues();
                await AMQPProvider.PublishAsync(sensor.ToJson(), "states");
                Thread.Sleep(100);
            }
        }
    }
}