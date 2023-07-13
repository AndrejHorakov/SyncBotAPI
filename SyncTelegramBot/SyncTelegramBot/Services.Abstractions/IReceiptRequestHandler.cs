using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IReceiptRequestHandler
{
    public void HandleDefault(PostReceiptToUNFModel model, string operationType, int amount);
    
    public Task HandleContragentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragent);
    
    public Task HandleDecryptionContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragentGuid);
    
    public Task HandleDecryptionDocumentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string document);
    
    public Task HandleCorrespondenceAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string correspondence);
}