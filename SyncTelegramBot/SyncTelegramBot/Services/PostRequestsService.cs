using System.Net;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services;

public static class PostRequestsService
{
    public static async Task<AnswerFromApi> SaveReceipt(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostReceipt));
    
    public static async Task<AnswerFromApi> SaveExpense(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostExpense));
    
    public static async Task<AnswerFromApi> SaveMove(DataForRequest dataForRequest) =>
        await Matcher(await ConfigureModel(dataForRequest, dataForRequest.UnfClient.PostMove));

    private static async Task<AnswerFromApi> Matcher(Result<Task<AnswerFromApi>, ValidationException> result) =>
        await result.Match<Task<AnswerFromApi>>(m => m, failure => Task.FromResult(new AnswerFromApi(failure.Message)));

    private static async Task<Result<Task<AnswerFromApi>, ValidationException>> ConfigureModel(DataForRequest dataForRequest,
        Func<PostToUnfModel?, Task<HttpResponseMessage?>> func)
    {
        dataForRequest.Model = new PostToUnfModel(dataForRequest.PostFromBotModel.Amount, dataForRequest.PostFromBotModel.OperationType, DateTime.Now);
        if (!StaticStructures.HandledOperations.ContainsKey(dataForRequest.PostFromBotModel.OperationType))
            return new ValidationException("Ошибка в названии операции, обратитесь к разработчикам");
        var resultOfHandling = await StaticStructures.HandledOperations[dataForRequest.PostFromBotModel.OperationType](dataForRequest);
        return resultOfHandling.Match(async handlerReadyModel =>
        {
            var ans = await func(handlerReadyModel.Model);
            return new AnswerFromApi
            {
                Answer = ans!.StatusCode == HttpStatusCode.OK
                    ? "Операция прошла успешно!"
                    : "Операция была прервана, произошла ошибка!"
            };
        }, 
            failure => Task.FromResult(new AnswerFromApi("Операция была прервана, произошла ошибка!" + '\n' + failure.Message)));
    }
}