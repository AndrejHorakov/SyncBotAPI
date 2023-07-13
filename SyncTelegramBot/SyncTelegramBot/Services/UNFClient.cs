using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UNFClient : IUNFClient
{
    private RequestStrings _requestStrings;
    private HttpClient _httpClient;
    private Uri _baseURI;

    public UNFClient(IOptions<RequestStrings> requestStrings)
    {
        _requestStrings = requestStrings.Value;
        _baseURI = new (_requestStrings.BaseUri);
        _httpClient = new HttpClient{BaseAddress = _baseURI};
        _httpClient.DefaultRequestHeaders.Add("Authorization", _requestStrings.Authorization);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<HttpResponseMessage> GetFromUNF(string filter)
    {
        return await _httpClient.GetAsync(_baseURI + filter);
    }
}