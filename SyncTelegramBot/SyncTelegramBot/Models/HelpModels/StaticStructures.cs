using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Models.HelpModels;

public static class StaticStructures
{
    public static readonly Dictionary<string, Type> Types = new()
    {
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUNF<VATRate>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Catalog_Организации"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Document_РасходИзКассы"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUNF<LoanAgreement>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUNF<Correspondence>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUNF<CodeAndDescription>),
    };

    public static readonly Dictionary<string, Func<IUNFClient, PostToUNFModel, PostFromBotModel, Task>>
        HandledOperations = new()
        {
            ["ОтПокупателя"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!);
                await ReceiptRequestHandler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!,
                    postBotModel.ContractFromDecryptionOfPayment!);
            },
            ["Прочее"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!);
            },
            ["ОтПоставщика"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!);
                await ReceiptRequestHandler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!);
                await ReceiptRequestHandler.HandleDecryptionDocumentAsync(model, unfClient,
                    postBotModel.DocumentFromDecryptionOfPayment!);
            },
            ["РасчетыПоКредитам"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!);
                await ReceiptRequestHandler.HandleLoanAgreementAsync(model, unfClient, postBotModel.LoanAgreement!);

            },
            ["ВозвратЗаймаСотрудником"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleLoanAgreementAsync(model, unfClient, postBotModel.LoanAgreement!);
                await ReceiptRequestHandler.HandleEmployeeAsync(model, unfClient, postBotModel.Employee!);
                ReceiptRequestHandler.HandleAmountTypeAsync(model, postBotModel.AmountType!);
            },
            ["ПокупкаВалюты"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!);
                await ReceiptRequestHandler.HandleCurrencyAsync(model, unfClient, postBotModel.Currency!);

            },
            ["ПолучениеНаличныхВБанке"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleOrganisationAccountAsync(model, unfClient,
                    postBotModel.OrganisationAccount!);
            },
            ["ПрочиеРасчеты"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!);
                await ReceiptRequestHandler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!);
                await ReceiptRequestHandler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!);

            },
            ["ОтПодотчетника"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleEmployeeAsync(model, unfClient, postBotModel.Employee!);
                await ReceiptRequestHandler.HandleContractAsync(model, unfClient, postBotModel.Contract!);

            },
            ["ЛичныеСредстваПредпринимателя"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleCorrespondenceAsync(model, unfClient, postBotModel.Correspondence!);
                await ReceiptRequestHandler.HandleOrganisationAsync(model, unfClient, postBotModel.Organisation!);

            },
            ["ОтНашейОрганизации"] = async (unfClient, model, postBotModel) =>
            {
                await ReceiptRequestHandler.HandleContragentAsync(model, unfClient, postBotModel.Contragent!);
                await ReceiptRequestHandler.HandleDecryptionContractAsync(model, unfClient, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!);

            },
        };

    public static Dictionary<string, Func<IUNFClient, string, Task<string?>>> HandleOptionKey = new()
    {
        ["Контрагент_Key"] = async (unfClient, contragent) => 
            await ReceiptRequestHandler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent),
        ["Owner"] = async (unfClient, contragent) => 
            await ReceiptRequestHandler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent),
        ["Сотрудник_Key"] = async (unfClient, contragent) => 
            await ReceiptRequestHandler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent),
        ["Автор_Key"] = async (unfClient, contragent) => 
            await ReceiptRequestHandler.HandleContragentAsync(new PostToUNFModel(), unfClient, contragent),
    };
}