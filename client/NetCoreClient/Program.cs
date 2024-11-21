using MQTTnet.Client;
using NetCoreClient.Protocols;
using NetCoreClient.Sensors;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main()
    {
        ClientServices cloudServices = new();
        cloudServices.Start();

        while (true) { }
    }
}

public class ClientServices()
{
    public MqttProvider? MqttProvider { get; private set; }
    public async Task Start()
    {
        MqttProvider = new("iot.scuola.iacca.ml", 1883, "mqtt-01");
        Dht11 sensor = new();
        if (MqttProvider != null && await MqttProvider.Connect())
        {
            if (!await MqttProvider.SubscribeTopic("iot/0001/commands/#"))
                Console.WriteLine("Errore sottoscrivendo al topic.");
            MqttProvider.MessageReceived += MessageReceived;
            u
            while (true)
            {
                sensor.ReadValues();
                await MqttProvider.Publish("iot/0001/data", sensor.ToJson());
                Thread.Sleep(1000);
            }
        }
    }

    private void MessageReceived(object? sender, EventArgs args)
    {
        var mqttArgs = (MqttApplicationMessageReceivedEventArgs)args;
        Console.WriteLine("Command received: " + mqttArgs.ApplicationMessage.Topic  + " " + Encoding.UTF8.GetString(mqttArgs.ApplicationMessage.PayloadSegment));
    }
}