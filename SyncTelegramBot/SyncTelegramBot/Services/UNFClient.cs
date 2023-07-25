using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using System.Net;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UnfClient : IUnfClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _baseUri;
    private readonly string _topOneFilter;

    public UnfClient(IOptions<RequestValues> requestStrings)
    {
        _topOneFilter = "&$top=1";
        var requestValues = requestStrings.Value;
        _baseUri = new (requestValues.BaseUri);
        _httpClient = new HttpClient{BaseAddress = _baseUri};
        _httpClient.DefaultRequestHeaders.Add("Authorization", requestValues.Authorization);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<HttpResponseMessage> GetFromUnf(string filter)
    {
        return await _httpClient.GetAsync(_baseUri + filter);
    }

    public async Task<string?> GetGuidFirst(string filter)
    {
        var respMess = await _httpClient.GetAsync(_baseUri + filter + _topOneFilter);
        var ans = await respMess.Content.ReadFromJsonAsync<AnswerFromUnf<GuidEntity>>();
        if (ans?.Value is null || ans.Value.Count <= 0)
            return null;
        return ans.Value[0].Guid;
    }

    public async Task<HttpResponseMessage?> PostReceipt(PostToUnfModel? model)
    {
        var ans =  await _httpClient.PostAsJsonAsync("Document_ПоступлениеВКассу?$format=json", model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return ans;
        var entity = await ans.Content.ReadFromJsonAsync<GuidEntity>();
        var guid = entity?.Guid;
        return await _httpClient.GetAsync($"Document_ПоступлениеВКассу(guid'{guid}')/Post");
    }
    
    public async Task<HttpResponseMessage?> PostExpense(PostToUnfModel? model)
    {
        var ans =  await _httpClient.PostAsJsonAsync("Document_РасходИзКассы?$format=json", model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return ans;
        var entity = await ans.Content.ReadFromJsonAsync<GuidEntity>();
        var guid = entity?.Guid;
        return await _httpClient.GetAsync($"Document_РасходИзКассы(guid'{guid}')/Post");
    }
    
    public async Task<HttpResponseMessage?> PostMove(PostToUnfModel? model)
    {
        var entityString = model?.OperationType == "МеждуКассами" ? "Document_ПеремещениеДС" : "Document_РасходСоСчета";
        var ans =  await _httpClient.PostAsJsonAsync(entityString, model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return ans;
        var entity = await ans.Content.ReadFromJsonAsync<GuidEntity>();
        var guid = entity?.Guid;
        return await _httpClient.GetAsync($"{entityString}(guid'{guid}')/Post");
    }
}