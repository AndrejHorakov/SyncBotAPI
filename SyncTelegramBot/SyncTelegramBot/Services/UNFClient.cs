using System.Net;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UNFClient : IUNFClient
{
    private HttpClient _httpClient;
    private static readonly Uri _baseURI = new Uri("https://1c.hightech.group/unf_sandbox/ru/odata/standard.odata/");
    private static readonly Uri _topOneFilter = new Uri("$top=1");

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

    public async Task<string?> GetGuidFirst(string filter)
    {
        var respMess = await _httpClient.GetAsync(_baseURI + filter);
        return (await respMess.Content.ReadFromJsonAsync<GuidEntity>())?.Guid;
    }

    public async Task<HttpResponseMessage?> PostReceipt(PostReceiptToUNFModel model)
    {
        var ans =  await _httpClient.PostAsJsonAsync("Document_ПоступлениеВКассу",model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return ans;
        var guid = (await ans.Content.ReadFromJsonAsync<GuidEntity>())?.Guid;
        return await _httpClient.GetAsync($"Document_ПоступлениеВКассу(guid'{guid}')/Post");
    }
}