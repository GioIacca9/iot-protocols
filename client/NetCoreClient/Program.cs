using NetCoreClient.Protocols;
using NetCoreClient.Sensors;

internal class Program
{
    public MQTT protocol;
    public static void Main(string[] args)
    {
        Program main = new();
        main.Run();
    }

    private void Run()
    {
        protocol = new MQTT("iot.scuola.iacca.ml", 1883, "mqtt-01", "", "");
        protocol.Connect();
        Dht11 sensor = new();

        while (true)
        {
            sensor.ReadValues();
            protocol.Send("iot/casetta/data", sensor.ToJson());
            Thread.Sleep(1000);
        }
    }
}