
using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using System.Net;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UNFClient : IUNFClient
{
    private RequestStrings _requestStrings;
    private HttpClient _httpClient;
    private Uri _baseURI;
    private static readonly Uri _topOneFilter = new Uri("$top=1");

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