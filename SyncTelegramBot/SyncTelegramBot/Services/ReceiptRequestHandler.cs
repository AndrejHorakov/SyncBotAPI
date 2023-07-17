using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class ReceiptRequestHandler
{
    private static double _defaultExchangeRate;
    private static double _defaultMultiplicity;

    public ReceiptRequestHandler(IOptions<RequestValues> rv)
    {
        _defaultMultiplicity = rv.Value.DefaultMultiplicity;
        _defaultExchangeRate = rv.Value.DefaultExchangeRate;
    }
   
    public static PostToUNFModel? HandleDefault(PostToUNFModel? model, string operationType, int amount)
    {
        if (model is null)
            model = new PostToUNFModel();
        model.Amount = amount;
        model.AmountCount = amount;
        model.OperationType = operationType;
        model.Date = DateTime.Now;
        return model;
    }

    public static async Task<string?> HandleContragentAsync(PostToUNFModel? model, IUNFClient unfClient, string contragent)
    {
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        var splitContragent = contragent.Split('*');
        model!.Contragent = await unfClient
            .GetGuidFirst($"Catalog_Контрагенты?$filter=Code eq '{splitContragent[0]}' and Description eq '{splitContragent[1]}'");
        return model.Contragent;
    }

    public static async Task<string?> HandleDecryptionContractAsync(PostToUNFModel? model, IUNFClient unfClient, string contragentGuid, string contractFromDecryption)
    {
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitDecrContract = contractFromDecryption.Split('*');
        model.Decryption[0].Contract = await unfClient.GetGuidFirst(
            $"Catalog_ДоговорыКонтрагентов?$filter=Owner eq cast(guid'{contragentGuid}', 'Catalog_Контрагенты') and Code eq '{splitDecrContract[0]}'");
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
        return model.Decryption[0].Contract;
    }

    public static async Task<string?> HandleDecryptionDocumentAsync(PostToUNFModel? model, IUNFClient unfClient, string document)
    {
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        var splitDecryption = document.Split('*');
        model.Decryption[0].DocumentType = "StandardODATA.Document_ПриходнаяНакладная";
        model.Decryption[0].Document = await unfClient.GetGuidFirst(
            $"Document_ПриходнаяНакладная?$filter=Number eq '{splitDecryption[0]}' and СуммаДокумента eq {splitDecryption[2]}");
        return model.Decryption[0].Document;
    }

    public static async Task<string?> HandleCorrespondenceAsync(PostToUNFModel? model, IUNFClient unfClient, string correspondence)
    {
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        var splitCorrespondence = correspondence.Split('*');
        model.Correspondence = await unfClient.GetGuidFirst(
            $"ChartOfAccounts_Управленческий?$filter=Description eq '{splitCorrespondence[1]}' and ТипСчета eq '{splitCorrespondence[2]}'");
        return model.Correspondence;
    }

    public static async Task<string?> HandleLoanAgreementAsync(PostToUNFModel? model, IUNFClient unfClient, string loanAgreement)
    {
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        var splitContragent = loanAgreement.Split('*');
        model.LoanAgreement = await unfClient.GetGuidFirst(
            $"Document_ДоговорКредитаИЗайма?$filter=Number eq '{splitContragent[0]}' and ВидДоговора eq '{splitContragent[1]}'");
        return model.LoanAgreement;
    }

    public static async Task<string?> HandleEmployeeAsync(PostToUNFModel? model, IUNFClient unfClient, string employee)
    {
        if (model is null)
            model = HandleDefault(model, "ОтПоставщика", 0);
        model.Employee = await unfClient.GetGuidFirst($"Catalog_Сотрудники?$filter=Description eq '{employee}'");
        return model.Employee;
    }

    public static async Task<string?> HandleCurrencyAsync(PostToUNFModel? model, IUNFClient unfClient, string currency)
    {
        if (model is null)
            model = HandleDefault(model, "ОтПоставщика", 0);
        var splitCurrency = currency.Split('*');
        model!.Currency = await unfClient.GetGuidFirst(
            $"Catalog_Валюты?$filter=СимвольноеПредставление eq '{splitCurrency[0]}' and Description eq '{splitCurrency[1]}'");
        model.ExchangeRate = (int?)_defaultExchangeRate;
        model.Multiplicity = _defaultMultiplicity;
        model.AmountCount = model.Amount * model.Multiplicity * model.ExchangeRate;
        return model.Currency;
    }

    public static async Task<string?> HandleOrganisationAsync(PostToUNFModel? model, IUNFClient unfClient, string organisation)
    {
        if (model is null)
            model = HandleDefault(model, "ОтПоставщика", 0);
        var ans =  await unfClient.GetGuidFirst(
            $"Catalog_Организации?$filter=Description eq '{organisation}'");
        model.Organisation = ans;
        return ans;
    }

    public static async Task<string?> HandleOrganisationAccountAsync(PostToUNFModel model, IUNFClient unfClient, string bankAccount)
    {
        if (model is null)
            model = HandleDefault(model, "ОтПоставщика", 0);
        var splitBankAccount = bankAccount.Split('*');
        var ans = await unfClient.GetGuidFirst(
            $"Catalog_БанковскиеСчета?$filter=Code eq '{splitBankAccount[0]}' and Description eq '{splitBankAccount[1]}'");
        model.OrganisationAccount = ans;
        return ans;
    }

    public static async Task<string?> HandleContractAsync(PostToUNFModel? model, IUNFClient unfClient, string contract)
    {
        if (model is null)
            model = HandleDefault(model, "ОтПоставщика", 0);
        var splitContract = contract.Split('*');
        model.Contract = await unfClient.GetGuidFirst(
            $"Document_РасходИзКассы?$filter=Number eq '{splitContract[0]}' and СуммаДокумента eq {splitContract[2]}");
        model.ContractType = "StandardODATA.Document_РасходИзКассы";
        return model.Contract;
    }
    public static void HandleAmountTypeAsync(PostToUNFModel? model, string amountType)
    {
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].AmountType = amountType;
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
    }
}