using SyncTelegramBot.Models.PostToUNFModels;

namespace SyncTelegramBot.Services.Abstractions;

public interface IReceiptRequestHandler
{
    public void HandleDefault(PostToUNFModel model, string operationType, int amount);
    
    public Task HandleContragentAsync(PostToUNFModel model, IUNFClient unfClient, string contragent);
    
    public Task HandleDecryptionContractAsync(PostToUNFModel model, IUNFClient unfClient, string contragentGuid, string contractFromDecryption);
    
    public Task HandleDecryptionDocumentAsync(PostToUNFModel model, IUNFClient unfClient, string document);
    
    public Task HandleCorrespondenceAsync(PostToUNFModel model, IUNFClient unfClient, string correspondence);
    
    public Task HandleLoanAgreementAsync(PostToUNFModel model, IUNFClient unfClient, string loanAgreement);
    
    public Task HandleEmployeeAsync(PostToUNFModel model, IUNFClient unfClient, string employee);
    
    public Task HandleCurrencyAsync(PostToUNFModel model, IUNFClient unfClient, string currency);
    
    public Task HandleOrganisationAsync(PostToUNFModel model, IUNFClient unfClient, string organisation);
    
    public Task HandleOrganisationAccountAsync(PostToUNFModel model, IUNFClient unfClient, string bankAccount);
    
    public Task HandleContractAsync(PostToUNFModel model, IUNFClient unfClient, string contract);
    
    public void HandleAmountTypeAsync(PostToUNFModel model, string amountType);
}