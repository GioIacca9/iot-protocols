using System.IO.Ports;
using System.Text.Json;

namespace NetCoreClient.Sensors;
class Dht11 : IAirSensorInterface, ISensorInterface, IDisposable
{
    private readonly SerialPort _serial;
    private double _airTemperature;
    private double _airHumidity;
    private Random _random;

    public double AirTemperature { get => _airTemperature;}
    public double AirHumidity { get => _airHumidity;}

    public event EventHandler<PropertyChangedEventArgs>? SerialDataReceived;

    public Dht11()
    {
        while (SerialPort.GetPortNames().Length < 1) { }
        _random = new Random();
        _serial = new(SerialPort.GetPortNames()[0], 9600);
        _serial.DataReceived += DataReceived;
        _serial.Open();
    }

    private void DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        _airTemperature = double.Parse(_serial.ReadLine()) / 100;
        _airHumidity = _random.NextDouble() * 100;

        SerialDataReceived?.Invoke(this, new PropertyChangedEventArgs(AirTemperature, AirHumidity));
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
public class PropertyChangedEventArgs(double newTemperature, double newHumidity) : EventArgs
{
    public double AirTemperature { get; private set; } = newTemperature;
    public double AirHumidity { get; private set; } = newHumidity;
}