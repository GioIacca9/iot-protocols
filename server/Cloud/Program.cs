﻿using InfluxDB.Client;
using InfluxDB.Client.Writes;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

namespace Cloud;
public class Program
{
    private static void Main()
    {
        CloudServices cloudServices = new();
        cloudServices.Start();

        while (true) { }
    }
}
public class CloudServices()
{
    private Data? dataReceived;
    public MqttProvider? MqttProvider { get; private set; }
    public async Task Start()
    {
        MqttProvider = new("iot.scuola.iacca.ml", 1883, "cloud");
        if (MqttProvider != null && await MqttProvider.Connect())
        {
            if (!await MqttProvider.SubscribeTopic("iot/0001/data"))
                Console.WriteLine("Errore sottoscrivendo al topic.");
            MqttProvider.MessageReceived += MessageReceived;
            await MqttProvider.Publish("iot/v1/0001/commands/externallight", "on");
        }
    }

    private void MessageReceived(object? sender, EventArgs args)
    {
        var mqttArgs = (MqttApplicationMessageReceivedEventArgs)args;
        var json = Encoding.UTF8.GetString(mqttArgs.ApplicationMessage.PayloadSegment);

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
            Console.WriteLine("sent");
        }
    }
}