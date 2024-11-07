namespace NetCoreClient.Protocols;
class Http(string endpoint) : IProtocolInterface
{
    public string Endpoint { get; set; } = endpoint;

    public async void Send(string data)
    {
        var client = new HttpClient();
        var result = await client.PostAsync(Endpoint, new StringContent(data));
        Console.Out.WriteLine(result.StatusCode);
    }
}
