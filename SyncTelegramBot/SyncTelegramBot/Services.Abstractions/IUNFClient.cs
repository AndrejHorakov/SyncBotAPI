using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IUNFClient
{
    public Task<HttpResponseMessage> GetFromUNF(string filter);

    public Task<string?> GetGuidFirst(string filter);
    
    public Task<HttpResponseMessage?> PostReceipt(PostToUNFModel model);
}