using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Models.HelpModels;

public static class StaticStructures
{
    public static readonly HashSet<string> AmountTypes = new()
    {
        "ОсновнойДолг", "Проценты", "Комиссия"
    };
    
    public static readonly Dictionary<string, Type> Types = new()
    {
        ["Catalog_Валюты"] = typeof(AnswerFromUNF<Currency>),
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUNF<VATRate>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Catalog_Организации"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Document_РасходИзКассы"] = typeof(AnswerFromUNF<Expense>),
        ["Document_РасходСоСчета"] = typeof(AnswerFromUNF<Expense>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUNF<LoanAgreement>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUNF<Correspondence>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUNF<CodeAndDescription>),
    };

    public static readonly Dictionary<string, Func<IUNFClient, PostToUNFModel, PostFromBotModel, ReceiptRequestHandler, Task<bool>>>
        HandledOperations = new()
        {
            ["ОтПокупателя"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!,
                        postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                return true;
            },
            ["Прочее"] = async (unfClient, model, postBotModel, handler) =>
            {
                return await handler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!) is not null;
            },
            ["ОтПоставщика"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                if (await handler.HandleDecryptionDocumentAsync(model, unfClient,
                        postBotModel.DocumentFromDecryptionOfPayment!) is null) return false;
                return true;
            },
            ["РасчетыПоКредитам"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!) is null) return false;;
                if (await handler.HandleLoanAgreementAsync(model, unfClient, postBotModel.LoanAgreement!) is null) return false;
                return true;
            },
            ["ВозвратЗаймаСотрудником"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleLoanAgreementAsync(model, unfClient, postBotModel.LoanAgreement!) is null) return false;
                if (await handler.HandleEmployeeAsync(model, unfClient, postBotModel.Employee!) is null) return false;
                if (handler.HandleAmountTypeAsync(model, postBotModel.AmountType!) is null) return false;
                return true;
            },
            ["ПокупкаВалюты"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleCurrencyAsync(model, unfClient, postBotModel.Currency!) is null) return false;
                return true;
            },
            ["ПолучениеНаличныхВБанке"] = async (unfClient, model, postBotModel, handler) =>
            {
                return await handler.HandleOrganisationAccountAsync(model, unfClient,
                    postBotModel.OrganisationAccount!) is not null;
            },
            ["ПрочиеРасчеты"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                return true;
            },
            ["ОтПодотчетника"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleEmployeeAsync(model, unfClient, postBotModel.Employee!) is null) return false;
                if (await handler.HandleContractAsync(model, unfClient, postBotModel.Contract!) is null) return false;
                return true;
            },
            ["ЛичныеСредстваПредпринимателя"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleOrganisationAsync(model, unfClient, postBotModel.Organisation!) is null) return false;
                return true;
            },
            ["ОтНашейОрганизации"] = async (unfClient, model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                return true;
            },
        };

    public static Dictionary<string, Func<IUNFClient, string, ReceiptRequestHandler, Task<string?>>> HandleOptionKey = new()
    {
        ["Контрагент_Key"] = async (unfClient, contragent, handler) => 
            $"guid'{await handler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent)}'",
        
        ["Owner"] = async (unfClient, contragent, handler) =>
        {
            var guid = await handler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent);
            return $"cast(guid'{guid}', 'Catalog_Контрагенты')";
        },
        
        ["Сотрудник_Key"] = async (unfClient, employee, handler) => 
            $"guid'{await handler.HandleEmployeeAsync(new PostToUNFModel(), unfClient, employee)}'",
        
        ["Подотчетник_Key"] = async (unfClient, employee, handler) => 
            $"guid'{await handler.HandleEmployeeAsync(new PostToUNFModel(), unfClient, employee)}'",
    };
}