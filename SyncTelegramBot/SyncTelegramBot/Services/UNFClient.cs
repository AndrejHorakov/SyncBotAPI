using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UNFClient : IUNFClient
{
    private HttpClient _httpClient;
    private static readonly Uri _baseURI = new Uri("https://1c.hightech.group/unf_sandbox/ru/odata/standard.odata/");

    public UNFClient()
    {
        _httpClient = new HttpClient{BaseAddress = _baseURI};
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic cGV0cm92Okp1MFZ1dGFt");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<HttpResponseMessage> GetFromUNF(string filter)
    {
        return await _httpClient.GetAsync(_baseURI + filter);
    }
}