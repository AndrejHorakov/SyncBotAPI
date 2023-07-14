using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class ReceiptRequestHandler : IReceiptRequestHandler
{
    private double _defaultExchangeRate = 92.15;
    private double _defaultMultiplicity = 1;
    public void HandleDefault(PostReceiptToUNFModel model, string operationType, int amount)
    {
        model.Amount = amount;
        model.AmountCount = amount;
        model.OperationType = operationType;
        model.Date = DateTime.Now;
    }

    public async Task HandleContragentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragent)
    {
        var splitedContragent = contragent.Split('*');
        model.Contragent = await unfClient
            .GetGuidFirst($"Catalog_Контрагенты?$filter=Code eq '{splitedContragent[0]}' and Description eq '{splitedContragent[1]}'");
    }

    public async Task HandleDecryptionContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contragentGuid, string contractFromDecryption)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitedDecrContract = contractFromDecryption.Split('*');
        model.Decryption[0].Contract = await unfClient.GetGuidFirst(
            $"Catalog_ДоговорыКонтрагентов?$filter=Owner eq cast(guid'{contragentGuid}', 'Catalog_Контрагенты') and Code eq '{splitedDecrContract[0]}'");
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
    }

    public async Task HandleDecryptionDocumentAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string document)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitedDecryption = document.Split('*');
        model.Decryption[0].DocumentType = "StandardODATA.Document_ПриходнаяНакладная";
        model.Decryption[0].Document = await unfClient.GetGuidFirst(
            $"Document_ПриходнаяНакладная?$filter=Number eq '{splitedDecryption[0]}' and СуммаДокумента eq {splitedDecryption[2]}");
    }

    public async Task HandleCorrespondenceAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string correspondence)
    {
        var splitCorrespondence = correspondence.Split('*');
        model.Correspondence = await unfClient.GetGuidFirst(
            $"ChartOfAccounts_Управленческий?$filter=Description eq '{splitCorrespondence[1]}' and ТипСчета eq '{splitCorrespondence[2]}'");
    }

    public async Task HandleLoanAgreementAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string loanAgreement)
    {
        var splitedContragent = loanAgreement.Split('*');
        model.LoanAgreement = await unfClient.GetGuidFirst(
            $"Document_ДоговорКредитаИЗайма?$filter=Number eq '{splitedContragent[0]}' and ВидДоговора eq '{splitedContragent[1]}'");
    }

    public async Task HandleEmployeeAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string employee)
    {
        model.Employee = await unfClient.GetGuidFirst($"Catalog_Сотрудники?$filter=Description eq '{employee}'");
    }

    public async Task HandleCurrencyAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string currency)
    {
        var splitedCurrency = currency.Split('*');
        model.Currency = await unfClient.GetGuidFirst(
            $"Catalog_Валюты?$filter=СимвольноеПредставление eq '{splitedCurrency[0]}' and Description eq '{splitedCurrency[1]}'");
        model.ExchangeRate = (int?)_defaultExchangeRate;
        model.Multiplicity = _defaultMultiplicity;
        model.AmountCount = model.Amount * model.Multiplicity * model.ExchangeRate;
    }

    public async Task HandleOrganisationAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string organisation)
    {
        var ans =  await unfClient.GetGuidFirst(
            $"Catalog_Организации?$filter=Description eq '{organisation}'");
        model.Organisation = ans;
    }

    public async Task HandleOrganisationAccountAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string bankAccount)
    {
        var splitedBankAccount = bankAccount.Split('*');
        var ans = await unfClient.GetGuidFirst(
            $"Catalog_БанковскиеСчета?$filter=Code eq '{splitedBankAccount[0]}' and Description eq '{splitedBankAccount[1]}'");
        model.OrganisationAccount = ans;
    }

    public async Task HandleContractAsync(PostReceiptToUNFModel model, IUNFClient unfClient, string contract)
    {
        var splitedContract = contract.Split('*');
        model.Contract = await unfClient.GetGuidFirst(
            $"Document_РасходИзКассы?$filter=Number eq '{splitedContract[0]}' and СуммаДокумента eq {splitedContract[2]}");
        model.ContractType = "StandardODATA.Document_РасходИзКассы";
    }
    public void HandleAmountTypeAsync(PostReceiptToUNFModel model, string amountType)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].AmountType = amountType;
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
    }
}