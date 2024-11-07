using System.Text;

namespace NetCoreClient.Protocols;

class Http(string endpoint) : IProtocolInterface
{
    public string Endpoint { get; set; } = endpoint;

    public async void Send(string data)
    {
        var client = new HttpClient();
        await client.PostAsync(Endpoint, new StringContent(data, Encoding.UTF8, "application/json"));
    }
}
