using System.Net;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services;

public static class PostRequestsService
{
    public static async Task<AnswerFromApi> SaveReceipt(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostReceipt!));
    
    public static async Task<AnswerFromApi> SaveExpense(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostExpense!));
    
    public static async Task<AnswerFromApi> SaveMove(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostMove!));

    private static async Task<AnswerFromApi> Matcher(Result<Task<AnswerFromApi>, ValidationException> result) =>
        await result.Match<Task<AnswerFromApi>>(m => m, failure => Task.FromResult(new AnswerFromApi(failure.Message)));

    private static async Task<Result<Task<AnswerFromApi>, ValidationException>> ConfigureModel(DataForRequest dataForRequest,
        Func<PostToUnfModel?, Task<Result<HttpResponseMessage?, ValidationException>>> requestMethod)
    {
        dataForRequest.Model = new PostToUnfModel(dataForRequest.PostFromBotModel.Amount, dataForRequest.PostFromBotModel.OperationType, DateTime.Now);
        if (!StaticStructures.HandledOperations.ContainsKey(dataForRequest.PostFromBotModel.OperationType))
            return new ValidationException("Ошибка в названии операции, обратитесь к разработчикам");
        var resultOfHandling = await StaticStructures.HandledOperations[dataForRequest.PostFromBotModel.OperationType](dataForRequest);
        return await resultOfHandling.Match<Task<Result<Task<AnswerFromApi>, ValidationException>>>(async handlerReadyModel =>
        {
            var ans = await requestMethod(handlerReadyModel.Model);
            return await ans.Match<Task<Result<Task<AnswerFromApi>, ValidationException>>>(async httpResponseMessage => await Task.FromResult(Task.FromResult(new AnswerFromApi
                {
                    Answer = httpResponseMessage!.StatusCode == HttpStatusCode.OK
                        ? "Операция прошла успешно!"
                        : "Операция была прервана, произошла ошибка!"
                })),
                async failure => await Task.FromResult(failure));
        }, async failure => await Task.FromResult(new ValidationException("Операция была прервана, произошла ошибка!" + '\n' + failure.Message)));
    }
}