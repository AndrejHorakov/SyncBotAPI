using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IReceiptRequestHandler
{
    public void HandleDefault(PostReceiptToUNFModel model, string operationType, int amount);
    
    public Task HandleContragentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragent);
    
    public Task HandleDecryptionContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragentGuid, string contractFromDecryption);
    
    public Task HandleDecryptionDocumentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string document);
    
    public Task HandleCorrespondenceAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string correspondence);
    
    public Task HandleLoanAgreementAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string loanAgreement);
    
    public Task HandleEmployeeAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string employee);
    
    public Task HandleCurrencyAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string currency);
    
    public Task HandleOrganisationAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string organisation);
    
    public Task HandleOrganisationAccountAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string bankAccount);
    
    public Task HandleContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contract);
    
    public void HandleAmountTypeAsync(PostReceiptToUNFModel model, string amountType);
    
}