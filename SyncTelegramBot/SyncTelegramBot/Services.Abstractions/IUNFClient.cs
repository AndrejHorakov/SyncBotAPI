using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IUnfClient
{
    public Task<HttpResponseMessage> GetFromUnf(string filter);

    public Task<string?> GetGuidFirst(string filter);
    
    public Task<HttpResponseMessage?> PostReceipt(PostToUnfModel? model);
    
    public Task<HttpResponseMessage?> PostExpense(PostToUnfModel? model);

    public Task<HttpResponseMessage?> PostMove(PostToUnfModel? model);
}