namespace NetCoreClient.ValueObjects;
internal class WaterTemperature(int value)
{
    public int Value { get; private set; } = value;
}
