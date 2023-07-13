using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IUNFClient
{
    public Task<HttpResponseMessage> GetFromUNF(string filter);

    public Task<string?> GetGiudFirst(string filter);
    
    public Task<HttpResponseMessage?> PostReceipt(PostReceiptToUNFModel model);
}