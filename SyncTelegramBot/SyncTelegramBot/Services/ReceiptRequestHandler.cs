using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class ReceiptRequestHandler
{
    private static double _defaultExchangeRate;
    private static double _defaultMultiplicity;

    public ReceiptRequestHandler(IOptions<RequestValues> rw)
    {
        _defaultMultiplicity = rw.Value.DefaultMultiplicity;
        _defaultExchangeRate = rw.Value.DefaultExchangeRate;
    }
   
    public static void HandleDefault(PostToUNFModel model, string operationType, int amount)
    {
        model.Amount = amount;
        model.AmountCount = amount;
        model.OperationType = operationType;
        model.Date = DateTime.Now;
    }

    public static async Task HandleContragentAsync(PostToUNFModel model, IUNFClient unfClient, string contragent)
    {
        var splitContragent = contragent.Split('*');
        model.Contragent = await unfClient
            .GetGuidFirst($"Catalog_Контрагенты?$filter=Code eq '{splitContragent[0]}' and Description eq '{splitContragent[1]}'");
    }

    public static async Task HandleDecryptionContractAsync(PostToUNFModel model, IUNFClient unfClient, string contragentGuid, string contractFromDecryption)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitDecrContract = contractFromDecryption.Split('*');
        model.Decryption[0].Contract = await unfClient.GetGuidFirst(
            $"Catalog_ДоговорыКонтрагентов?$filter=Owner eq cast(guid'{contragentGuid}', 'Catalog_Контрагенты') and Code eq '{splitDecrContract[0]}'");
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
    }

    public static async Task HandleDecryptionDocumentAsync(PostToUNFModel model, IUNFClient unfClient, string document)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitDecryption = document.Split('*');
        model.Decryption[0].DocumentType = "StandardODATA.Document_ПриходнаяНакладная";
        model.Decryption[0].Document = await unfClient.GetGuidFirst(
            $"Document_ПриходнаяНакладная?$filter=Number eq '{splitDecryption[0]}' and СуммаДокумента eq {splitDecryption[2]}");
    }

    public static async Task HandleCorrespondenceAsync(PostToUNFModel model, IUNFClient unfClient, string correspondence)
    {
        var splitCorrespondence = correspondence.Split('*');
        model.Correspondence = await unfClient.GetGuidFirst(
            $"ChartOfAccounts_Управленческий?$filter=Description eq '{splitCorrespondence[1]}' and ТипСчета eq '{splitCorrespondence[2]}'");
    }

    public static async Task HandleLoanAgreementAsync(PostToUNFModel model, IUNFClient unfClient, string loanAgreement)
    {
        var splitContragent = loanAgreement.Split('*');
        model.LoanAgreement = await unfClient.GetGuidFirst(
            $"Document_ДоговорКредитаИЗайма?$filter=Number eq '{splitContragent[0]}' and ВидДоговора eq '{splitContragent[1]}'");
    }

    public static async Task HandleEmployeeAsync(PostToUNFModel model, IUNFClient unfClient, string employee)
    {
        model.Employee = await unfClient.GetGuidFirst($"Catalog_Сотрудники?$filter=Description eq '{employee}'");
    }

    public static async Task HandleCurrencyAsync(PostToUNFModel model, IUNFClient unfClient, string currency)
    {
        var splitCurrency = currency.Split('*');
        model.Currency = await unfClient.GetGuidFirst(
            $"Catalog_Валюты?$filter=СимвольноеПредставление eq '{splitCurrency[0]}' and Description eq '{splitCurrency[1]}'");
        model.ExchangeRate = (int?)_defaultExchangeRate;
        model.Multiplicity = _defaultMultiplicity;
        model.AmountCount = model.Amount * model.Multiplicity * model.ExchangeRate;
    }

    public static async Task HandleOrganisationAsync(PostToUNFModel model, IUNFClient unfClient, string organisation)
    {
        var ans =  await unfClient.GetGuidFirst(
            $"Catalog_Организации?$filter=Description eq '{organisation}'");
        model.Organisation = ans;
    }

    public static async Task HandleOrganisationAccountAsync(PostToUNFModel model, IUNFClient unfClient, string bankAccount)
    {
        var splitBankAccount = bankAccount.Split('*');
        var ans = await unfClient.GetGuidFirst(
            $"Catalog_БанковскиеСчета?$filter=Code eq '{splitBankAccount[0]}' and Description eq '{splitBankAccount[1]}'");
        model.OrganisationAccount = ans;
    }

    public static async Task HandleContractAsync(PostToUNFModel model, IUNFClient unfClient, string contract)
    {
        var splitContract = contract.Split('*');
        model.Contract = await unfClient.GetGuidFirst(
            $"Document_РасходИзКассы?$filter=Number eq '{splitContract[0]}' and СуммаДокумента eq {splitContract[2]}");
        model.ContractType = "StandardODATA.Document_РасходИзКассы";
    }
    public static void HandleAmountTypeAsync(PostToUNFModel model, string amountType)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].AmountType = amountType;
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
    }
}