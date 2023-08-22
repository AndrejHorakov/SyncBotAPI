using System.Net;
using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class UnfClient : IUnfClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _baseUri;
    private readonly string _topOneFilter;

    public UnfClient(IOptions<RequestValues> requestStrings, HttpClient client)
    {
        _topOneFilter = "&$top=1";
        var requestValues = requestStrings.Value;
        _baseUri = new (requestValues.BaseUri);
        _httpClient = client;
    }

    public async Task<Result<HttpResponseMessage, ValidationException>?> GetFromUnf(string filter)
    {
        var result = await _httpClient.GetAsync(_baseUri + filter);
        return (int)result.StatusCode switch
        {
            < 500 and >= 400 => new ValidationException("Ошибка на стороне клиента при запросе"),
            >= 500 => new ValidationException("Внутренняя ошибка сервера"),
            _ => result
        };
    }

    public async Task<string?> GetGuidFirst(string filter)
    {
        var respMess = await _httpClient.GetAsync(_baseUri + filter + _topOneFilter);
        var ans = await respMess.Content.ReadFromJsonAsync<AnswerFromUnf<GuidEntity>>();
        if (ans?.Value is null || ans.Value.Count <= 0)
            return null;
        return ans.Value[0].Guid;
    }

    public async Task<Result<HttpResponseMessage, ValidationException>?> PostReceipt(PostToUnfModel? model) =>
        await ProcessPostRequest("Document_ПоступлениеВКассу", model);

    public async Task<Result<HttpResponseMessage, ValidationException>?> PostExpense(PostToUnfModel? model) =>
        await ProcessPostRequest("Document_РасходИзКассы", model);

    public async Task<Result<HttpResponseMessage, ValidationException>?> PostMove(PostToUnfModel? model) => 
        await ProcessPostRequest(model?.OperationType == "МеждуКассами" ? "Document_ПеремещениеДС" : "Document_РасходСоСчета", model);

    private async Task<Result<HttpResponseMessage, ValidationException>> ProcessPostRequest(string requestEntity, PostToUnfModel? model)
    {
        var ans =  await _httpClient.PostAsJsonAsync(requestEntity, model);
        if (ans.StatusCode != HttpStatusCode.Created)
            return new ValidationException("При создании объекта произошла ошибка, проверьте введенные данные");
        var entity = await ans.Content.ReadFromJsonAsync<GuidEntity>();
        var guid = entity?.Guid;
        var result = await _httpClient.GetAsync($"{requestEntity}(guid'{guid}')/Post");
        if (result.StatusCode != HttpStatusCode.OK)
            return new ValidationException("Операция сохранена, произошла ошибка при проведении операции, проверьте введенные данные");
        return result;
    }
}