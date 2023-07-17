using System.Net;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class PostReceiveRequestHandler
{
    public async Task<AnswerFromAPI> SaveReceipt(IUNFClient unfClient, PostFromBotModel postModel)
    {
        var model = new PostToUNFModel();
        ReceiptRequestHandler.HandleDefault(model, postModel.OperationType, postModel.Amount);
        
        if (StaticStructures.HandledOperations.ContainsKey(postModel.OperationType))
            await StaticStructures.HandledOperations[postModel.OperationType](unfClient, model, postModel);
        else
            return new AnswerFromAPI
            {
                Answer = "Ошибка в названии операции, обратитесь к разработчикам"
            };
        
        var ans = await unfClient.PostReceipt(model);
        return new AnswerFromAPI
        {
            Answer = ans!.StatusCode == HttpStatusCode.OK
                ? "Операция прошла успешно!"
                : "Операция была прервана, произошла ошибка!"
        };
    }
}