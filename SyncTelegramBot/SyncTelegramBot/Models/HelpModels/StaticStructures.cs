using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Models.HelpModels;

public static class StaticStructures
{
    public static readonly Dictionary<string, Func<PostToUNFModel, string, PostType, IUNFClient, Task>> DocumentInfo = new()
    {
        ["Накладная"] =  (model, document, type, unfClient) =>  type switch
            {
                PostType.Expense => HandleDecryptionAsync(5, model, document,
                    unfClient,"СуммаДокумента",  2, str=> str,  "Расходная"),
                _ => HandleDecryptionAsync(5, model, document, unfClient, "СуммаДокумента",2, str=> str, "Приходная")
            },
        ["ЗаказПокупателя"] = async (model, document, type, unfClient) => await HandleDecryptionAsync(3, model, document,
            unfClient, "ВидОперации", 1, str => $"'{str}'"),
        ["ПоступлениеНаСчет"] = async (model, document, type, unfClient) => await HandleDecryptionAsync(4, model, document,
            unfClient, "СуммаДокумента", 2, str => str),
        ["АктВыполненныхРабот"] = async (model, document, type, unfClient) => await HandleDecryptionAsync(3, model, document,
            unfClient , "ИдентификаторПлатежа", 1, str => $"'{str}'"),
        ["КорректировкаРеализации"] = async (model, document, type, unfClient) => await HandleDecryptionAsync(3, model, document,
            unfClient, "ВидОперации", 1, str => $"'{str}'"),
    };

    private static async Task<string?> HandleDecryptionAsync(int slices, PostToUNFModel model,  string? entity,
        IUNFClient unfClient, string secondParameter, int numberSecondParameter, Func<string, string> formatter, string? typeInvoice = null)
    {
        var splitDecryption = entity?.Split('*');
        if (splitDecryption?.Length != slices)
            return null;
        model.Decryption ??= new DecryptionPayment[] { new() { LineNumber = "1" } };
        model.Decryption[0].DocumentType = $"StandardODATA.Document_{typeInvoice+splitDecryption[^1]}";
        model.Decryption[0].Document = await unfClient.GetGuidFirst(
            $"Document_{typeInvoice+splitDecryption[^1]}?$filter=Number eq '{splitDecryption[0]}' and {secondParameter} eq {formatter(splitDecryption[numberSecondParameter])}");
        //СуммаДокумента
        return model.Decryption[0].Document;
    }
    
    public static readonly HashSet<string> AmountTypes = new()
    {
        "ОсновнойДолг", "Проценты", "Комиссия"
    };
    
    public static readonly Dictionary<string, Type> Types = new()
    {
        ["Catalog_Валюты"] = typeof(AnswerFromUNF<Currency>),
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUNF<VATRate>),
        ["Document_РасходСоСчета"] = typeof(AnswerFromUNF<Expense>),
        ["Document_РасходИзКассы"] = typeof(AnswerFromUNF<Expense>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Catalog_Организации"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Document_ЗаказПокупателя"] = typeof(AnswerFromUNF<BuyerOrder>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUNF<Invoice>),
        ["Document_РасходнаяНакладная"] = typeof(AnswerFromUNF<Invoice>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUNF<LoanAgreement>),
        ["Document_ПоступлениеНаСчет"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUNF<Correspondence>),
        ["Document_АктВыполненныхРабот"] = typeof(AnswerFromUNF<ActCompletedWork>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Document_КорректировкаРеализации"] = typeof(AnswerFromUNF<CorrectImplementation>),
        ["Document_ПоступлениеНаСчет?$filter=ВидОперации eq 'ОтПокупателя'"] = typeof(AnswerFromUNF<ReceiveInAccount>),
    };
    
    public static readonly Dictionary<string, List<string>> ListEntities = new()
    {
        ["Catalog_Валюты"] = new(){"Catalog_Валюты"},
        ["Catalog_СтавкиНДС"] = new(){"Catalog_СтавкиНДС"},
        ["Catalog_Сотрудники"] = new(){"Catalog_Сотрудники"},
        ["Catalog_Организации"] = new(){"Catalog_Организации"},
        ["Catalog_ВидыНалогов"] = new(){"Catalog_ВидыНалогов"},
        ["Catalog_Контрагенты"] = new(){"Catalog_Контрагенты"},
        ["Document_РасходИзКассы"] = new(){"Document_РасходИзКассы"},
        ["Document_РасходСоСчета"] = new(){"Document_РасходСоСчета"},
        ["Catalog_БанковскиеСчета"] = new(){"Catalog_БанковскиеСчета"},
        ["Document_ПоступлениеНаСчет"] = new(){"Document_ПоступлениеНаСчет"},
        ["Document_ПриходнаяНакладная"] = new(){"Document_ПриходнаяНакладная"},
        ["Catalog_ДоговорыКонтрагентов"] = new(){"Catalog_ДоговорыКонтрагентов"},
        ["Document_ДоговорКредитаИЗайма"] = new(){"Document_ДоговорКредитаИЗайма"},
        ["ChartOfAccounts_Управленческий"] = new(){"ChartOfAccounts_Управленческий"},
        ["Document_Покупателю"] = new()
        {
            "Document_РасходнаяНакладная",
            "Document_КорректировкаРеализации",
            "Document_ПоступлениеНаСчет?$filter=ВидОперации eq 'ОтПокупателя'",
            "Document_АктВыполненныхРабот",
            "Document_ЗаказПокупателя"
        },
    };

    public static readonly Dictionary<string, Func<PostToUNFModel, PostFromBotModel, ReceiptRequestHandler, Task<bool>>>
        HandledOperations = new()
        {
            ["ОтПокупателя"] = async (model, postBotModel, handler) =>
            {
                // var init = Option<T>();
                // model.TryAddContragent()
                //     .AddDescription()
                //     .adfsljsdaflsda();
                
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, model.Contragent!,
                        postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                return true;
            },
            ["Прочее"] = async (model, postBotModel, handler) =>
            {
                return await handler.HandleCorrespondenceAsync(model, postBotModel.Correspondence!) is not null;
            },
            ["ОтПоставщика"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                if (await handler.HandleDecryptionDocumentAsync(model,
                        postBotModel.DocumentFromDecryptionOfPayment!, postBotModel.Type) is null) return false;
                return true;
            },
            ["РасчетыПоКредитам"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleLoanAgreementAsync(model, postBotModel.LoanAgreement!) is null) return false;

                if (postBotModel.Type is PostType.Expense)
                {
                    if (await handler.HandleDecryptionContractAsync(model, model.Contragent!,
                            postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                    if (await handler.HandleVATRateAsync(model, postBotModel.VATRate!) is null) return false;
                    if ( handler.HandleAmountType(model, postBotModel.AmountType!) is null) return false;
                }
                
                return true;
            },
            ["ВозвратЗаймаСотрудником"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleLoanAgreementAsync(model, postBotModel.LoanAgreement!) is null) return false;
                if (await handler.HandleEmployeeAsync(model, postBotModel.Employee!) is null) return false;
                if (handler.HandleAmountType(model, postBotModel.AmountType!) is null) return false;
                return true;
            },
            ["ПокупкаВалюты"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleCorrespondenceAsync(model, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleCurrencyAsync(model, postBotModel.Currency!) is null) return false;
                return true;
            },
            ["ПолучениеНаличныхВБанке"] = async (model, postBotModel, handler) =>
            {
                return await handler.HandleOrganisationAccountAsync(model,
                    postBotModel.OrganisationAccount!) is not null;
            },
            ["ПрочиеРасчеты"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleCorrespondenceAsync(model, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                if (postBotModel.Type is PostType.Expense)
                    if (await handler.HandleVATRateAsync(model, postBotModel.VATRate!) is null) return false;
                return true;
            },
            ["ОтПодотчетника"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleEmployeeAsync(model, postBotModel.Employee!) is null) return false;
                if (await handler.HandleContractAsync(model, postBotModel.Contract!) is null) return false;
                return true;
            },
            ["ЛичныеСредстваПредпринимателя"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleCorrespondenceAsync(model, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleOrganisationAsync(model, postBotModel.Organisation!) is null) return false;
                return true;
            },
            ["ОтНашейОрганизации"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleDecryptionContractAsync(model, model.Contragent!, postBotModel.ContractFromDecryptionOfPayment!) is null) return false;
                return true;
            },
            
            
            ["Поставщику"] = async (model, postBotModel, handler) =>
            {
                if (!await HandledOperations["ОтПокупателя"](model, postBotModel, handler)) return false;
                if (await handler.HandleVATRateAsync(model, postBotModel.VATRate!) is null) return false;
                return true;
            },
            ["НаРасходы"] = async (model, postBotModel, handler) =>
            {
                return await HandledOperations["Прочее"](model, postBotModel, handler);
            },
            ["НашейОрганизации"] = async (model, postBotModel, handler) =>
            {
                return await HandledOperations["Поставщику"](model, postBotModel, handler);
            }, 
            ["ЗарплатаСотруднику"] = async (model, postBotModel, handler) =>
            {
                return await handler.HandleEmployeeAsync(model, postBotModel.Employee) is not null;
            },
            ["Покупателю"] = async (model, postBotModel, handler) =>
            {
                if (!await HandledOperations["ОтПоставщика"](model, postBotModel, handler)) return false;
                if (await handler.HandleVATRateAsync(model, postBotModel.VATRate!) is null) return false;
                return true;
            }, 
            ["ВыдачаЗаймаСотруднику"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleEmployeeAsync(model, postBotModel.Employee!) is null) return false;
                if (await handler.HandleLoanAgreementAsync(model, postBotModel.LoanAgreement!) is null) return false;
                return true;
            },
            ["ВзносНаличнымиВБанк"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleCorrespondenceAsync(model, postBotModel.Correspondence!) is null) return false;
                if (await handler.HandleOrganisationAccountAsync(model, postBotModel.OrganisationAccount!) is null) return false;
                return true;
            },
            ["Подотчетнику"] = async (model, postBotModel, handler) =>
            {
                return await HandledOperations["ЗарплатаСотруднику"](model, postBotModel, handler);
            },
            ["Налоги"] = async (model, postBotModel, handler) =>
            {
                if (await handler.HandleContragentAsync(model, postBotModel.Contragent!) is null) return false;
                if (await handler.HandleTaxTypeAsync(model, postBotModel.TypeOfTax!) is null) return false;
                return true;
            },
            ["МеждуКассами"] = async (model, postBotModel, handler) =>
            {
                return true;
            },
            ["МеждуСчетами"] = async (model, postBotModel, handler) =>
            {
                handler.HandleDefault(model, "ПереводНаДругойСчет", postBotModel.Amount);
                if (await handler.HandleOrganisationAccountAsync(model, postBotModel.ContragentAccount!) is null) return false;
                model.ContragentAccount = model.OrganisationAccount;
                model.OrganisationAccount = null;
                return true;
            },
        };

    public static Dictionary<string, Func<string, ReceiptRequestHandler, Task<string?>>> HandleOptionKey = new()
    {
        ["Контрагент_Key"] = async (contragent, handler) => 
            $"guid'{await handler.HandleContragentAsync(new PostToUNFModel(), contragent)}'",
        
        ["Owner"] = async (contragent, handler) =>
        {
            var guid = await handler.HandleContragentAsync(new PostToUNFModel(), contragent);
            return $"cast(guid'{guid}', 'Catalog_Контрагенты')";
        },
        
        ["Сотрудник_Key"] = async (employee, handler) => 
            $"guid'{await handler.HandleEmployeeAsync(new PostToUNFModel(), employee)}'",
        
        ["Подотчетник_Key"] = async (employee, handler) => 
            $"guid'{await handler.HandleEmployeeAsync(new PostToUNFModel(), employee)}'",
        
        ["ВидНалога_Key"] = async (taxType, handler) => 
            $"guid'{await handler.HandleTaxTypeAsync(new PostToUNFModel(), taxType)}'",
        
        ["СтавкаНДС_Key"] = async (vatRate, handler) => 
            $"guid'{await handler.HandleVATRateAsync(new PostToUNFModel(), vatRate)}'",
        
        ["СчетКонтрагента_Key"] = async (contragentAccount, handler) => 
            $"guid'{await handler.HandleOrganisationAccountAsync(new PostToUNFModel(), contragentAccount)}'",
    };
}