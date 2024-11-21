using System;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace NetCoreClient.Protocols;

public class MQTT
{
	private IMqttClient _mqttClient;
	private IMqttClientOptions _options;

	public void Send(string topic, string data)
	{
		if (_mqttClient.IsConnected)
		{
			_mqttClient.PublishAsync(topic, data).Wait();
		}
	}

	public MQTT(string hostname, int port, string clientId, string username, string password)
	{
		var factory = new MqttFactory();

		_mqttClient = factory.CreateMqttClient();
		_options = new MqttClientOptionsBuilder()
			.WithTcpServer(hostname, port) // MQTT broker address and port
			.WithCredentials(username, password) // Set username and password
			.WithClientId(clientId)
			.WithCleanSession()
			.Build();

	}

	public async void Connect()
	{
		await _mqttClient.ConnectAsync(_options);
	}
}
