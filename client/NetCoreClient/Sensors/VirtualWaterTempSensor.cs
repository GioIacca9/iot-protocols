using NetCoreClient.ValueObjects;
using System.Text.Json;

namespace NetCoreClient.Sensors;
class VirtualWaterTempSensor : IWaterTempSensorInterface, ISensorInterface
{
    private readonly Random _random;

    public VirtualWaterTempSensor()
    {
        _random = new();
    }

    public int WaterTemperature()
    {
        return new WaterTemperature(_random.Next(20)).Value;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(WaterTemperature());
    }
}
