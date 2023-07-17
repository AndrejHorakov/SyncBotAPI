using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IReceiptRequestHandler
{
    public void HandleDefault(PostToUNFModel model, string operationType, int amount);
    
    public Task HandleContragentAsync(PostToUNFModel model, IUNFClient unfClient, string contragent);
    
    public Task HandleDecryptionContractAsync(PostToUNFModel model, IUNFClient unfClient, string contragentGuid);
    
    public Task HandleDecryptionDocumentAsync(PostToUNFModel model, IUNFClient unfClient, string document);
    
    public Task HandleCorrespondenceAsync(PostToUNFModel model, IUNFClient unfClient, string correspondence);
}