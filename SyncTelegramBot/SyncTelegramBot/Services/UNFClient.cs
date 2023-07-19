
using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using System.Net;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UNFClient : IUNFClient
{
    private RequestValues _requestValues;
    private HttpClient _httpClient;
    private Uri _baseURI;
    private static readonly Uri _topOneFilter = new Uri("$top=1");

    public UNFClient(IOptions<RequestValues> requestStrings)
    {
        _requestValues = requestStrings.Value;
        _baseURI = new (_requestValues.BaseUri);
        _httpClient = new HttpClient{BaseAddress = _baseURI};
        _httpClient.DefaultRequestHeaders.Add("Authorization", _requestValues.Authorization);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<HttpResponseMessage> GetFromUNF(string filter)
    {
        return await _httpClient.GetAsync(_baseURI + filter);
    }

    public async Task<string?> GetGuidFirst(string filter)
    {
        var respMess = await _httpClient.GetAsync(_baseURI + filter);
        var ans = await respMess.Content.ReadFromJsonAsync<AnswerFromUNF<GuidEntity>>();
        if (ans?.Value.Count <= 0)
            return null;
        return ans?.Value[0].Guid;
    }

    public async Task<HttpResponseMessage?> PostReceipt(PostToUNFModel? model)
    {
        var ans =  await _httpClient.PostAsJsonAsync("Document_ПоступлениеВКассу?$format=json", model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return ans;
        var entity = await ans.Content.ReadFromJsonAsync<GuidEntity>();
        var guid = entity?.Guid;
        return await _httpClient.GetAsync($"Document_ПоступлениеВКассу(guid'{guid}')/Post");
    }
}