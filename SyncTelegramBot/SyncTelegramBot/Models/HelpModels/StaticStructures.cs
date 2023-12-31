using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Services;

namespace SyncTelegramBot.Models.HelpModels;

public static class StaticStructures
{
    // public static readonly Dictionary<string, Func<DataForRequest, Task<Result<DataForRequest, ValidationException>>>> DocumentInfo = new()
    // {
    //     ["Накладная"] =  handler =>  handler.PostFromBotModel.Type switch
    //         {
    //             PostType.Expense => HandleDecryptionAsync(
    //                 5, handler,"СуммаДокумента",  2, str=> str,  "Расходная"),
    //             _ => HandleDecryptionAsync(
    //                 5, handler, "СуммаДокумента",2, str=> str, "Приходная")
    //         },
    //     ["ЗаказПокупателя"] = async handler =>
    //         await HandleDecryptionAsync(3, handler, "ВидОперации", 1, str => $"'{str}'"),
    //     ["ПоступлениеНаСчет"] =
    //         async handler => await HandleDecryptionAsync(4, handler, "СуммаДокумента", 2, str => str),
    //     ["АктВыполненныхРабот"] = async handler =>
    //         await HandleDecryptionAsync(3, handler, "ИдентификаторПлатежа", 1, str => $"'{str}'"),
    //     ["КорректировкаРеализации"] = async handler =>
    //         await HandleDecryptionAsync(3, handler, "ВидОперации", 1, str => $"'{str}'"),
    // };

    // private static async Task<Result<DataForRequest, ValidationException>> HandleDecryptionAsync(int slices, DataForRequest dataForRequest,
    //     string secondParameter, int indexSecondParameter, Func<string, string> formatter, string? typeInvoice = null)
    // {
    //     var splitDecryption = dataForRequest.PostFromBotModel.DocumentFromDecryptionOfPayment?.Split('*');
    //     if (splitDecryption?.Length != slices)
    //         return new ValidationException("Неверно введен документ");
    //     dataForRequest.Model.Decryption ??= new DecryptionPayment[] { new() { LineNumber = "1" } };
    //     dataForRequest.Model.Decryption[0].DocumentType = $"StandardODATA.Document_{typeInvoice+splitDecryption[^1]}";
    //     dataForRequest.Model.Decryption[0].Document = await dataForRequest.UnfClient.GetGuidFirst(
    //         $"Document_{typeInvoice+splitDecryption[^1]}?$filter=Number eq '{splitDecryption[0]}' and {secondParameter} eq {formatter(splitDecryption[indexSecondParameter])} and Контрагент_Key eq guid'{dataForRequest.Model.Contragent}'");
    //
    //     if (dataForRequest.Model.Decryption[0].Document is null)
    //         return new ValidationException("Документ не найден");
    //     //СуммаДокумента
    //     return dataForRequest;
    // }
    
    public static readonly HashSet<string> AmountTypes = new()
    {
        "ОсновнойДолг", "Проценты", "Комиссия"
    };
    
    public static readonly Dictionary<string, Type> Types = new()
    {
        ["Catalog_Валюты"] = typeof(AnswerFromUnf<Currency>),
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUnf<VatRate>),
        ["Document_РасходСоСчета"] = typeof(AnswerFromUnf<Expense>),
        ["Document_РасходИзКассы"] = typeof(AnswerFromUnf<Expense>),
        ["Document_ЗаказПокупателя"] = typeof(AnswerFromUnf<BuyerOrder>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUnf<Invoice>),
        ["Document_РасходнаяНакладная"] = typeof(AnswerFromUnf<Invoice>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUnf<DescriptionEntity>),
        ["Document_АвансовыйОтчет"] = typeof(AnswerFromUnf<AdvanceReport>),
        ["Catalog_Организации"] = typeof(AnswerFromUnf<DescriptionEntity>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUnf<CodeAndDescription>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUnf<CodeAndDescription>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUnf<CodeAndDescription>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUnf<LoanAgreement>),
        ["Document_ПоступлениеНаСчет"] = typeof(AnswerFromUnf<CodeAndDescription>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUnf<Correspondence>),
        ["Document_АктВыполненныхРабот"] = typeof(AnswerFromUnf<ActCompletedWork>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUnf<CodeAndDescription>),
        ["Document_КорректировкаРеализации"] = typeof(AnswerFromUnf<CorrectImplementation>),
        ["Document_ПоступлениеНаСчет?$filter=ВидОперации eq 'ОтПокупателя'"] = typeof(AnswerFromUnf<ReceiveInAccount>),
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
        ["Document_Поставщику"] = new ()
        {
            "Document_АвансовыйОтчет",
            "Document_ПриходнаяНакладная"
        },
        ["Document_Расход"] = new()
        {
            "Document_РасходИзКассы",
            "Document_РасходСоСчета"
        },
        ["Document_Покупателю"] = new()
        {
            "Document_РасходнаяНакладная",
            "Document_КорректировкаРеализации",
            "Document_ПоступлениеНаСчет?$filter=ВидОперации eq 'ОтПокупателя'",
            "Document_АктВыполненныхРабот",
            "Document_ЗаказПокупателя"
        },
    };

    public static readonly Dictionary<string, Func<Result<DataForRequest, ValidationException>, Task<Result<DataForRequest, ValidationException>>>>
        HandledOperations = new()
        {
            ["ОтПокупателя"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleDecryptionContractAsync();
                return handlerResult;
            },
            ["Прочее"] = async handlerResult =>
            {
                await handlerResult.HandleCorrespondenceAsync();
                return handlerResult;
            },
            ["ОтПоставщика"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleDecryptionContractAsync();
                await handlerResult.HandleDecryptionDocumentAsync();
                return handlerResult;
            },
            ["РасчетыПоКредитам"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleLoanAgreementAsync($"Контрагент_Key eq guid'{handlerResult.Value?.Model.Contragent}'");

                return await handlerResult.Match(async handler =>
                {
                    if (handler.PostFromBotModel.Type is PostType.Expense)
                    {
                        await handlerResult.HandleDecryptionContractAsync();
                        await handlerResult.HandleVatRateAsync();
                        await handlerResult.HandleAmountType();
                    }
                    return handlerResult;
                },
                failure => Task.FromResult<Result<DataForRequest, ValidationException>>(failure));
            },
            ["ВозвратЗаймаСотрудником"] = async handlerResult =>
            {
                await handlerResult.HandleEmployeeAsync();
                await handlerResult.HandleLoanAgreementAsync($"Сотрудник_Key eq guid'{handlerResult.Value?.Model.Employee}'");
                await handlerResult.HandleAmountType();
                return handlerResult;
            },
            ["ПокупкаВалюты"] = async handlerResult =>
            {
                await handlerResult.HandleCorrespondenceAsync();
                await handlerResult.HandleCurrencyAsync();
                return handlerResult;
            },
            ["ПолучениеНаличныхВБанке"] = async handlerResult =>
            {
                await handlerResult.HandleOrganisationAccountAsync();
                return handlerResult;
            },
            ["ПрочиеРасчеты"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleCorrespondenceAsync();
                await handlerResult.HandleDecryptionContractAsync();
                return await handlerResult.Match(async handler =>
                {
                    if (handler.PostFromBotModel.Type is PostType.Expense)
                        await handlerResult.HandleVatRateAsync();
                    return handlerResult;
                },
                    failure => Task.FromResult<Result<DataForRequest, ValidationException>>(failure));
            },
            ["ОтПодотчетника"] = async handlerResult =>
            {
                await handlerResult.HandleEmployeeAsync();
                await handlerResult.HandleContractAsync($"Сотрудник_Key eq guid'{handlerResult.Value?.Model.Employee}'");
                return handlerResult;
            },
            ["ЛичныеСредстваПредпринимателя"] = async handlerResult =>
            {
                await handlerResult.HandleCorrespondenceAsync();
                await handlerResult.HandleOrganisationAsync();
                return handlerResult;
            },
            ["ОтНашейОрганизации"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleDecryptionContractAsync();
                return handlerResult;
            },

            ["Поставщику"] = async handlerResult =>
            {
                await HandledOperations["ОтПокупателя"](handlerResult);
                await handlerResult.HandleVatRateAsync();
                return handlerResult;
            },
            ["НаРасходы"] = async handlerResult =>
            {
                await HandledOperations["Прочее"](handlerResult);
                return handlerResult;
            },
            ["НашейОрганизации"] = async handlerResult =>
            {
                await HandledOperations["Поставщику"](handlerResult);
                return handlerResult;
            }, 
            ["ЗарплатаСотруднику"] = async handlerResult =>
            {
                await handlerResult.HandleEmployeeAsync();
                return handlerResult;
            },
            ["Покупателю"] = async handlerResult =>
            {
                await HandledOperations["ОтПоставщика"](handlerResult);
                await handlerResult.HandleVatRateAsync();
                return handlerResult;
            }, 
            ["ВыдачаЗаймаСотруднику"] = async handlerResult =>
            {
                await handlerResult.HandleEmployeeAsync();
                await handlerResult.HandleLoanAgreementAsync($"Сотрудник_Key eq guid'{handlerResult.Value?.Model.Employee}'");
                return handlerResult;
            },
            ["ВзносНаличнымиВБанк"] = async handlerResult =>
            {
                await handlerResult.HandleCorrespondenceAsync();
                await handlerResult.HandleOrganisationAccountAsync();
                return handlerResult;
            },
            ["Подотчетнику"] = async handlerResult =>
            {
                await HandledOperations["ЗарплатаСотруднику"](handlerResult);
                return handlerResult;
            },
            ["Налоги"] = async handlerResult =>
            {
                await handlerResult.HandleContragentAsync();
                await handlerResult.HandleTaxTypeAsync();
                await handlerResult.HandlePaymentDeadline();
                return handlerResult;
            },
            ["МеждуКассами"] = Task.FromResult,
            ["МеждуСчетами"] = async handlerResult =>
            {
                return await handlerResult.Match(async handler =>
                {
                    handler.Model = new (handler.PostFromBotModel.Amount, "ПереводНаДругойСчет", DateTime.Now);
                    await handlerResult.HandleOrganisationAccountAsync();
                        
                    handler.Model.ContragentAccount = handler.Model.OrganisationAccount;
                    handler.Model.OrganisationAccount = null;
                    return handlerResult;
                },
                    failure => Task.FromResult<Result<DataForRequest, ValidationException>>(failure));
            },
        };

    // public static readonly Dictionary<string, Func<Result<DataForRequest, ValidationException>, string, Task<Result<string,ValidationException>>>> HandleOptionKey = new()
    // {
    //     ["Контрагент_Key"] = async (handler, entity) =>
    //     {
    //         handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { Contragent = entity }, new PostToUnfModel(),
    //             0, 0));
    //         var result = await handler.HandleContragentAsync();
    //         return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.Contragent!);
    //     },
    //     
    //     ["Owner"] = async (handler, entity) =>
    //     {
    //         handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { Contragent = entity }, new PostToUnfModel(),
    //             0, 0));
    //         var result = await handler.HandleContragentAsync();
    //         return await HandleResult(result, str => $"cast(guid'{str}', 'Catalog_Контрагенты')", handler.Value!.Model.Contragent!);
    //     },
    //     
    //     ["Сотрудник_Key"] = async (handler, entity) =>
    //         {
    //             handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { Employee = entity }, new PostToUnfModel(),
    //                 0, 0));
    //             var result = await handler.HandleEmployeeAsync();
    //             return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.Employee!);
    //         },
    //     
    //     ["Подотчетник_Key"] = async (handler, entity) =>
    //         {
    //             handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { Employee = entity }, new PostToUnfModel(),
    //                 0, 0));
    //             var result = await handler.HandleEmployeeAsync();
    //             return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.Employee!);
    //         },
    //
    //     ["ВидНалога_Key"] = async (handler, entity) =>
    //         {
    //             handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { TypeOfTax = entity }, new PostToUnfModel(),
    //                 0, 0));
    //             var result = await handler.HandleTaxTypeAsync();
    //             return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.TypeOfTax!);
    //         },
    //     
    //     ["СтавкаНДС_Key"] = async (handler, entity) =>
    //         {
    //             handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { VatRate = entity }, new PostToUnfModel(),
    //                 0, 0));
    //             var result = await handler.HandleVatRateAsync();
    //             return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.Decryption![0].VatRate!);
    //         },
    //     
    //     ["СчетКонтрагента_Key"] = async (handler, entity) =>
    //         {
    //             handler.ChangeValue(new DataForRequest(handler.Value!.UnfClient, new() { OrganisationAccount = entity }, new PostToUnfModel(),
    //                 0, 0));
    //             var result = await handler.HandleOrganisationAccountAsync();
    //             return await HandleResult(result, str => $"guid'{str}'", handler.Value!.Model.OrganisationAccount!);
    //         },
    // };

    // private static async Task<Result<string, ValidationException>> HandleResult(Result<DataForRequest, ValidationException> result,
    //     Func<string, string> formatter, string entity) => await result.Match<Task<Result<string, ValidationException>>>(
    //     _ => Task.FromResult<Result<string, ValidationException>>(formatter(entity)),
    //     failure =>
    //     {
    //         result.ErrorWasOccured(failure);
    //         return Task.FromResult<Result<string, ValidationException>>("");
    //     });
}