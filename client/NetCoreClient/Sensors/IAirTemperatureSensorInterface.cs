namespace NetCoreClient.Sensors;
interface IAirSensorInterface
{
    public double AirTemperature { get; }
    public double AirHumidity { get; }
}
