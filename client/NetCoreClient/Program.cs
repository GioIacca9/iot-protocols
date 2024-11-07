using NetCoreClient.Protocols;
using NetCoreClient.Sensors;

internal class Program
{
    public IProtocolInterface? protocol;
    public static void Main(string[] args)
    {
        Program main = new ();
        main.Run();
    }

    private void Run()
    {
        protocol = new Http("https://iot.scuola.iacca.ml/temperature");
        //protocol = new Http("https://dioenac.requestcatcher.com/");
        Dht11 sensor = new();
        List<ISensorInterface> sensors = [sensor];
        sensor.SerialDataReceived += AirTemperatureChanged;

        while (true) { }
    }

    private void AirTemperatureChanged(object? sender, PropertyChangedEventArgs e)
    {
        Dht11 dht11 = (Dht11)sender;
        string sensorValue = dht11.ToJson();
        protocol?.Send(sensorValue);
        Console.WriteLine("Data sent: " + sensorValue);
    }
}