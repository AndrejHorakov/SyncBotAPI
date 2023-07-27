using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IUnfClient
{
    public Task<Result<HttpResponseMessage, ValidationException>?> GetFromUnf(string filter);

    public Task<string?> GetGuidFirst(string filter);
    
    public Task<Result<HttpResponseMessage, ValidationException>?> PostReceipt(PostToUnfModel? model);
    
    public Task<Result<HttpResponseMessage, ValidationException>?> PostExpense(PostToUnfModel? model);

    public Task<Result<HttpResponseMessage, ValidationException>?> PostMove(PostToUnfModel? model);
}