using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class ReceiptRequestHandler
{
    private  double _defaultExchangeRate;
    private  double _defaultMultiplicity;
    private  RequestValues _requestValues;
    private  IUNFClient _unfClient;

    public ReceiptRequestHandler(IOptions<RequestValues> requestStrings, IUNFClient unfClient)
    {
        _unfClient = unfClient;
        _requestValues = requestStrings.Value;
        _defaultMultiplicity = _requestValues.DefaultMultiplicity;
        _defaultExchangeRate = _requestValues.DefaultExchangeRate;
    }
   
    public PostToUNFModel? HandleDefault(PostToUNFModel? model, string operationType, int amount)
    {
        model ??= new PostToUNFModel();

        model.Amount = amount;
        model.AmountCount = amount;
        model.OperationType = operationType;
        model.Date = DateTime.Now;
        return model;
    }

    public async Task<string?> HandleContragentAsync(PostToUNFModel? model, string contragent)
    {
        var splitContragent = ValidateModelAndInput(model, contragent, 2);
        if (!splitContragent.Item1)
            return null;
        model!.Contragent = await _unfClient
            .GetGuidFirst($"Catalog_Контрагенты?$filter=Code eq '{splitContragent.Item2[0]}' and Description eq '{splitContragent.Item2[1]}'");
        return model.Contragent;
    }

    public async Task<string?> HandleDecryptionContractAsync(PostToUNFModel? model, string contragentGuid, string contractFromDecryption)
    {
        var splitDecrContract = ValidateModelAndInput(model, contractFromDecryption, 2);
        if (!splitDecrContract.Item1)
            return null;
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].Contract = await _unfClient.GetGuidFirst(
            $"Catalog_ДоговорыКонтрагентов?$filter=Owner eq cast(guid'{contragentGuid}', 'Catalog_Контрагенты') and Code eq '{splitDecrContract.Item2[0]}'");
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
        return model.Decryption[0].Contract;
    }

    public async Task<string?> HandleDecryptionDocumentAsync(PostToUNFModel? model, string document, PostType type)
    {
        var splitDocument = document.Split('*');
        if (!StaticStructures.DocumentInfo.TryGetValue(splitDocument[^1], out var func))
            return null;
        await func(model, document, type, _unfClient);
        return model.Decryption[0].Document;
    }
    
    public async Task<string?> HandleCorrespondenceAsync(PostToUNFModel? model, string correspondence)
    {
        var splitCorrespondence = ValidateModelAndInput(model, correspondence, 3);
        if (!splitCorrespondence.Item1)
            return null;
      
        model.Correspondence = await _unfClient.GetGuidFirst(
            $"ChartOfAccounts_Управленческий?$filter=Description eq '{splitCorrespondence.Item2[1]}' and ТипСчета eq '{splitCorrespondence.Item2[2]}'");
        return model.Correspondence;
    }

    public async Task<string?> HandleLoanAgreementAsync(PostToUNFModel? model, string loanAgreement)
    {
        var splitLoan = ValidateModelAndInput(model, loanAgreement, 3);
        if (!splitLoan.Item1)
            return null;
        if (model is null)
            model = HandleDefault(model, "Прочее", 0);
        model.LoanAgreement = await _unfClient.GetGuidFirst(
            $"Document_ДоговорКредитаИЗайма?$filter=Number eq '{splitLoan.Item2[0]}' and ВидДоговора eq '{splitLoan.Item2[1]}'");
        return model.LoanAgreement;
    }

    public async Task<string?> HandleEmployeeAsync(PostToUNFModel? model, string employee)
    {
        var splitContragent = ValidateModelAndInput(model, employee, 1);
        if (!splitContragent.Item1)
            return null;
        model.Employee = await _unfClient.GetGuidFirst($"Catalog_Сотрудники?$filter=Description eq '{employee}'");
        return model.Employee;
    }

    public async Task<string?> HandleCurrencyAsync(PostToUNFModel? model, string currency)
    {
        var splitCurrency = ValidateModelAndInput(model, currency, 2);
        if (!splitCurrency.Item1)
            return null;
        model!.Currency = await _unfClient.GetGuidFirst(
            $"Catalog_Валюты?$filter=СимвольноеПредставление eq '{splitCurrency.Item2[0]}' and Description eq '{splitCurrency.Item2[1]}'");
        model.ExchangeRate = _defaultExchangeRate;
        model.Multiplicity = _defaultMultiplicity;
        model.AmountCount = model.Amount * model.Multiplicity * model.ExchangeRate;
        return model.Currency;
    }

    public async Task<string?> HandleOrganisationAsync(PostToUNFModel? model, string organisation)
    {
        var splitCurrency = ValidateModelAndInput(model, organisation, 1);
        if (!splitCurrency.Item1)
            return null;
        var ans =  await _unfClient.GetGuidFirst(
            $"Catalog_Организации?$filter=Description eq '{organisation}'");
        model.Organisation = ans;
        return ans;
    }

    public async Task<string?> HandleOrganisationAccountAsync(PostToUNFModel model, string bankAccount)
    {
        var splitBankAccount = ValidateModelAndInput(model, bankAccount, 2);
        if (!splitBankAccount.Item1)
            return null;
        model.OrganisationAccount = await _unfClient.GetGuidFirst(
            $"Catalog_БанковскиеСчета?$filter=Code eq '{splitBankAccount.Item2[0]}' and Description eq '{splitBankAccount.Item2[1]}'");
        return model.OrganisationAccount;
    }

    public async Task<string?> HandleContractAsync(PostToUNFModel? model, string contract)
    {
        var splitContract = ValidateModelAndInput(model, contract, 4);
        if (!splitContract.Item1)
            return null;
        splitContract.Item2[3] = splitContract.Item2[3] == "Касса" ? "ИзКассы" : "СоСчета";
        model.Contract = await _unfClient.GetGuidFirst(
            $"Document_Расход{splitContract.Item2[3]}?$filter=Number eq '{splitContract.Item2[0]}' and СуммаДокумента eq {splitContract.Item2[2]}");
        model.ContractType = "StandardODATA.Document_РасходИзКассы";
        return model.Contract;
    }
    
    public async Task<string?> HandleVATRateAsync(PostToUNFModel? model, string? vatRate)
    {
        var splitVatRate = ValidateModelAndInput(model, vatRate, 2);
        if (!splitVatRate.Item1)
            return null;
        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].VATRate = await _unfClient.GetGuidFirst(
            $"Catalog_СтавкиНДС?$filter=ВидСтавкиНДС eq '{splitVatRate.Item2[0]}' and Description eq '{splitVatRate.Item2[1]}'");
        return model.Decryption[0].VATRate;
    }
    
    public async Task<string?> HandleTaxTypeAsync(PostToUNFModel? model, string? taxType)
    {
        var splitTaxType = ValidateModelAndInput(model, taxType, 2);
        if (!splitTaxType.Item1)
            return null;
        model!.TypeOfTax = await _unfClient.GetGuidFirst(
            $"Catalog_ВидыНалогов?$filter=Code eq '{splitTaxType.Item2[0]}' and Description eq '{splitTaxType.Item2[1]}'");
        return model.TypeOfTax;
    }
    
    public string HandleAmountType(PostToUNFModel? model, string amountType)
    {
        if (!StaticStructures.AmountTypes.Contains(amountType))
            return null;

        if (model.Decryption is null)
            model.Decryption = new DecryptionPayment[]{new (){ LineNumber = "1"}};
        model.Decryption[0].AmountType = amountType;
        model.Decryption[0].AmountPayment = model.Amount;
        model.Decryption[0].AmountCount = model.Amount;
        return model.Decryption[0].AmountType!;
    }

    private Tuple<bool, string[]> ValidateModelAndInput(PostToUNFModel? model, string entity, int countSlices)
    {
        if (model is null)
            HandleDefault(model, "Default", 0);
        var splitEntity = entity.Split('*');
        if (splitEntity.Length != countSlices)
            return Tuple.Create<bool, string[]>(false, null);
        return Tuple.Create(true, splitEntity);

    }
}