using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using IOTAPI.Models;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/temperature", (Dht11 data) =>
{
    var token = config.GetConnectionString("InfluxToken");
    using var client = new InfluxDBClient("http://localhost:8086", token);
    var point = PointData
      .Measurement("sensors")
      .Tag("house_id", "1")
      .Field("temperature", data.AirTemperature)
      .Field("humidity", data.AirHumidity)
      .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

    using var writeApi = client.GetWriteApi();
    writeApi.WritePoint(point, "iotprotocol", "its");
})
.WithName("Post Temperature")
.WithOpenApi();

app.Run();