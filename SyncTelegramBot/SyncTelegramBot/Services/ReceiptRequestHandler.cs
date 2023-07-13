using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class ReceiptRequestHandler : IReceiptRequestHandler
{
    public void HandleDefault(PostReceiptToUNFModel model, string operationType, int amount)
    {
        model.Amount = amount;
        model.OperationType = operationType;
        model.Date = DateTime.Now;
    }

    public async Task HandleContragentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragent)
    {
        var splitedContragent = contragent.Split(' ');
        model.Contragent = await unfClient
            .GetGuidFirst($"Catalog_Контрагенты?$filter=Description eq {splitedContragent[0]} and Code eq {splitedContragent[1]}");
    }

    public async Task HandleDecryptionContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragentGuid)
    {
        if (model.Decryption is null)
            model.Decryption = new() { LineNumber = "1" };
        model.Decryption.Contract = await unfClient.GetGuidFirst(
            $"Catalog_ДоговорыКонтрагентов?$filter=Контрагент_Key eq '{contragentGuid}'");
        model.Decryption.AmountPayment = model.Amount;
        model.Decryption.AmountCount = model.Amount;
    }

    public async Task HandleDecryptionDocumentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string document)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment{ LineNumber = "1" };
        model.Decryption.DocumentType = "StandardODATA.Document_ПриходнаяНакладная";
        //model.Decryption.Document = await ...
    }

    public async Task HandleCorrespondenceAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string correspondence)
    {
        var splitedContragent = correspondence.Split(' ');
        model.Correspondence = await unfClient.GetGuidFirst(
            $"ChartOfAccounts_Управленческий?$filter=Description eq {splitedContragent[1]} and ТипСчета eq {splitedContragent[2]}");
    }
}