using InfluxDB.Client;
using InfluxDB.Client.Writes;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Cloud;
public class Program
{
    private static void Main()
    {
        CloudServices cloudServices = new();
        cloudServices.Start().GetAwaiter().GetResult();

        while (true) { }
    }
}
public class CloudServices()
{
    private Data? dataReceived;
    public AMQPProvider? AMQPProvider { get; private set; }
    public async Task Start()
    {
        AMQPProvider = new();
        try
        {
            if (AMQPProvider != null)
            {
                AMQPProvider.MessageReceived += MessageReceived;
                Console.WriteLine("AMQP Provider started.");
                await AMQPProvider.Start();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Impossibile creare la connessione con il server AMQP");
            throw;
        }
    }

    private void MessageReceived(object? sender, BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var json = Encoding.UTF8.GetString(body);

        if (json != null)
            dataReceived = JsonSerializer.Deserialize<Data>(json);

        if (dataReceived != null)
        {
            var token = "3PMH3rzcqS-fVEB4I6-IkVojAw89bv6LUaSweMhMKMBt5QNGcFmDby04sPyzWx9QfYn3r1ektWrYHzb7jCakww==";
            using var client = new InfluxDBClient("http://10.80.1.15:8086", token);
            var point = PointData
                .Measurement("sensors")
                .Tag("house_id", "0001")
                .Field("temperature", dataReceived.AirTemperature)
                .Field("humidity", dataReceived.AirHumidity)
                .Timestamp(DateTime.Now, InfluxDB.Client.Api.Domain.WritePrecision.Ms);

            using var writeApi = client.GetWriteApi();
            writeApi.WritePoint(point, "iotprotocol", "its");
            Console.WriteLine(json);
        }
    }
}