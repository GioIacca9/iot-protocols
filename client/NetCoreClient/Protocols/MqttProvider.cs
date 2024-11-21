using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using MQTTnet.Protocol;

namespace NetCoreClient.Protocols;
public class MqttProvider : IDisposable
{
    public string Broker { get; set; }
    public int Port { get; set; }
    public string ClientId { get; set; }
    private IMqttClient? mqttClient;
    private bool disposed;
    public event EventHandler MessageReceived;

    public MqttProvider(string broker, int port, string clientId)
    {
        if (!string.IsNullOrWhiteSpace(broker) && port != 0 && !string.IsNullOrWhiteSpace(clientId))
        {
            Broker = broker;
            Port = port;
            ClientId = clientId;
            mqttClient = null;
        }
    }

    public async Task<bool> Connect()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();

        // Create MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(Broker, Port)
            .WithClientId(ClientId)
            .WithCleanSession()
            .Build();

        var connectResult = await mqttClient.ConnectAsync(options);
        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to MQTT broker successfully.");
            return true;
        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
            return true;
        }
    }

    public async Task<bool> SubscribeTopic(string topic)
    {
        if (topic != null && mqttClient != null)
        {
            await mqttClient.SubscribeAsync(topic);
            mqttClient.ApplicationMessageReceivedAsync += MessageReceivedFromBroker;
            return true;
        }
        else return false;
    }
    private Task MessageReceivedFromBroker(MqttApplicationMessageReceivedEventArgs args)
    {
        MessageReceived.Invoke(this, args);
        return Task.CompletedTask;
    }

    public async Task<bool> Publish(string topic, string payload)
    {
        if (topic != null && mqttClient != null)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await mqttClient.PublishAsync(message);
            return true;
        }
        else return false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing && mqttClient != null)
            {
                mqttClient.UnsubscribeAsync(new MqttClientUnsubscribeOptions { TopicFilters = ["#"] });
                mqttClient.DisconnectAsync();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
