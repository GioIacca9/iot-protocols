using System.IO.Ports;
using System.Text.Json;

namespace NetCoreClient.Sensors;
class Dht11 : IAirSensorInterface, ISensorInterface, IDisposable
{
    private double _airTemperature;
    private double _airHumidity;
    private DateTime _readingDate;
    private Random _random;

    public double AirTemperature { get => _airTemperature; }
    public double AirHumidity { get => _airHumidity; }
    public DateTime ReadingDate { get => _readingDate; }

    public Dht11()
    {
        _random = new Random();
    }

    public void ReadValues()
    {
        _airTemperature = _random.NextDouble() * 100;
        _airHumidity = _random.NextDouble() * 100;
        _readingDate = DateTime.Now;
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
