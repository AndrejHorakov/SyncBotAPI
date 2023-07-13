using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IGetRequestHandler
{
    public Task<AnswerFromAPI> GetList(IUNFClient unfClient, string filter);
}