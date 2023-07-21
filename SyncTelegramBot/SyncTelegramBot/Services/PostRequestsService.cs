using System.Net;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class PostRequestsService
{
    public async Task<AnswerFromAPI> SaveReceipt(IUNFClient unfClient, PostFromBotModel postModel, ReceiptRequestHandler handler)
    {
        var model = new PostToUNFModel();
        handler.HandleDefault(model, postModel.OperationType, postModel.Amount);

        if (StaticStructures.HandledOperations.ContainsKey(postModel.OperationType))
        {
            if(!await StaticStructures.HandledOperations[postModel.OperationType](model, postModel, handler))
                return new AnswerFromAPI
                {
                    Answer = "Ошибка в введённых данных, проверьте данные и повторите"
                };
        }     
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
    
    public async Task<AnswerFromAPI> SaveExpense(IUNFClient unfClient, PostFromBotModel postModel, ReceiptRequestHandler handler)
    {
        var model = new PostToUNFModel();
        handler.HandleDefault(model, postModel.OperationType, postModel.Amount);

        if (StaticStructures.HandledOperations.ContainsKey(postModel.OperationType))
        {
            if(!await StaticStructures.HandledOperations[postModel.OperationType](model, postModel, handler))
                return new AnswerFromAPI
                {
                    Answer = "Ошибка в введённых данных, проверьте данные и повторите"
                };
        }     
        else
            return new AnswerFromAPI
            {
                Answer = "Ошибка в названии операции, обратитесь к разработчикам"
            };
        
        var ans = await unfClient.PostExpense(model);
        return new AnswerFromAPI
        {
            Answer = ans!.StatusCode == HttpStatusCode.OK
                ? "Операция прошла успешно!"
                : "Операция была прервана, произошла ошибка!"
        };
    }
    
    public async Task<AnswerFromAPI> SaveMove(IUNFClient unfClient, PostFromBotModel postModel, ReceiptRequestHandler handler)
    {
        var model = new PostToUNFModel();
        handler.HandleDefault(model, postModel.OperationType, postModel.Amount);

        if (StaticStructures.HandledOperations.ContainsKey(postModel.OperationType))
        {
            if(!await StaticStructures.HandledOperations[postModel.OperationType](model, postModel, handler))
                return new AnswerFromAPI
                {
                    Answer = "Ошибка в введённых данных, проверьте данные и повторите"
                };
        }     
        else
            return new AnswerFromAPI
            {
                Answer = "Ошибка в названии операции, обратитесь к разработчикам"
            };
        
        var ans = await unfClient.PostMove(model);
        return new AnswerFromAPI
        {
            Answer = ans!.StatusCode == HttpStatusCode.OK
                ? "Операция прошла успешно!"
                : "Операция была прервана, произошла ошибка!"
        };
    }
}