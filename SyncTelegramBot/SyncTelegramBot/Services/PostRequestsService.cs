using System.Net;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services;

public static class PostRequestsService
{
    public static async Task<AnswerFromApi> SaveReceipt(Handler handler) =>
        await Matcher(await ConfigureModel(handler, handler.UnfClient.PostReceipt));
    
    public static async Task<AnswerFromApi> SaveExpense(Handler handler) =>
        await Matcher(await ConfigureModel(handler, handler.UnfClient.PostExpense));
    
    public static async Task<AnswerFromApi> SaveMove(Handler handler) =>
        await Matcher(await ConfigureModel(handler, handler.UnfClient.PostMove));

    private static async Task<AnswerFromApi> Matcher(Result<Task<AnswerFromApi>, ValidationException> result) =>
        await result.Match<Task<AnswerFromApi>>(m => m, failure => Task.FromResult(new AnswerFromApi(failure.Message)));

    private static async Task<Result<Task<AnswerFromApi>, ValidationException>> ConfigureModel(Handler handler,
        Func<PostToUnfModel?, Task<HttpResponseMessage?>> func)
    {
        handler.Model = new PostToUnfModel(handler.PostFromBotModel.Amount, handler.PostFromBotModel.OperationType, DateTime.Now);
        if (!StaticStructures.HandledOperations.ContainsKey(handler.PostFromBotModel.OperationType))
            return new ValidationException("Ошибка в названии операции, обратитесь к разработчикам");
        var resultOfHandling = await StaticStructures.HandledOperations[handler.PostFromBotModel.OperationType](handler);
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