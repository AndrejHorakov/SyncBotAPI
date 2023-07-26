using System.Globalization;
using SyncTelegramBot.Models.Exceptions;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services;

public static class ResultPostModelExtensions
{
    public static async Task<Result<DataForRequest, ValidationException>> HandleContragentAsync(
        this Result<DataForRequest, ValidationException> handlerResult) =>
        await Matcher(handlerResult, async handler =>
        {
            var splitContragent = ValidateModelAndInput(handler.PostFromBotModel.Contragent!, 2);
            return await splitContragent.Match(async splitArray =>
                {
                    var guid = await handler.UnfClient
                        .GetGuidFirst(
                            $"Catalog_Контрагенты?$filter=Code eq '{splitArray[0]}' and Description eq '{splitArray[1]}'");

                    if (guid is null)
                        handlerResult.ErrorWasOccured(new ValidationException("Контрагент не найден"));

                    handler.Model.Contragent = guid;
                    return handlerResult;
                },
                failure =>
                {
                    handlerResult.ErrorWasOccured(new ValidationException($"Неверно введён контрагент: \n{failure.Message}"));
                    return Task.FromResult(handlerResult);
                });
        });

    public static async Task<Result<DataForRequest, ValidationException>> HandleDecryptionContractAsync(
        this Result<DataForRequest, ValidationException> handlerResult) =>
        await Matcher(handlerResult, async handler =>
        {
            var splitDecrContract = ValidateModelAndInput(handler.PostFromBotModel.ContractFromDecryptionOfPayment!, 2);
            return await splitDecrContract.Match(async decrContractParams =>
            {
                handler.Model.Decryption ??= new DecryptionPayment[] { new() { LineNumber = "1" } };
                handler.Model.Decryption[0].Contract = await handler.UnfClient.GetGuidFirst(
                    $"Catalog_ДоговорыКонтрагентов?$filter=Owner eq cast(guid'{handler.Model.Contragent}', 'Catalog_Контрагенты') and Code eq '{decrContractParams[0]}'");

                if (handler.Model.Decryption[0].Contract is null)
                {
                    handlerResult.ErrorWasOccured(new ValidationException("Контракт контрагента не найден"));
                    return handlerResult;
                }

                handler.Model.Decryption[0].AmountPayment = handler.Model.Amount;
                handler.Model.Decryption[0].AmountCount = handler.Model.Amount;
                return handlerResult;
            },
                failure =>
                {
                    handlerResult.ErrorWasOccured(new ValidationException($"Неверно введен контракт контрагента: \n{failure.Message}"));
                    return Task.FromResult(handlerResult);
                });
        });


    public static async Task<Result<DataForRequest, ValidationException>> HandleDecryptionDocumentAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => 
        await Matcher(handlerResult, async handler =>
        {
            var splitDocument = handler.PostFromBotModel.DocumentFromDecryptionOfPayment!.Split('*');
                if (!StaticStructures.DocumentInfo.TryGetValue(splitDocument[^1], out var func))
            {
                handlerResult.ErrorWasOccured(new ValidationException("Название документа не найдено"));
                return handlerResult;
            }
            
            var result = await func(handler);
            return await result.Match( handlerProcessed =>
                {
                    handlerResult.ChangeValue(handlerProcessed);
                    return Task.FromResult(handlerResult);
                },
                failure =>
                {
                    handlerResult.ErrorWasOccured(failure);
                    return Task.FromResult(handlerResult);
                });
        });



    public static async Task<Result<DataForRequest, ValidationException>> HandleCorrespondenceAsync(
        this Result<DataForRequest, ValidationException> handlerResult) =>
        await Matcher(handlerResult, async handler =>
        {
            var splitCorrespondence = ValidateModelAndInput(handler.PostFromBotModel.Correspondence, 3);
            return await splitCorrespondence.Match(async splitArray =>
                {
                    handler.Model.Correspondence = await handler.UnfClient.GetGuidFirst(
                        $"ChartOfAccounts_Управленческий?$filter=Description eq '{splitArray[1]}' and ТипСчета eq '{splitArray[2]}'");
                    
                    if (handler.Model.Correspondence is null)
                        handlerResult.ErrorWasOccured(new ValidationException("Корреспонденция не найдена"));
                    
                    return handlerResult;
                },
                failure =>
                {
                    handlerResult.ErrorWasOccured(new ValidationException($"Неверно введена корреспонденция: \n{failure.Message}"));
                    return Task.FromResult(handlerResult);
                });
        });



    public static async Task<Result<DataForRequest, ValidationException>> HandleLoanAgreementAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitLoan = ValidateModelAndInput(handler.PostFromBotModel.LoanAgreement, 3);
        return await splitLoan.Match(async splitLoanAgreement =>
        {
            handler.Model.LoanAgreement = await handler.UnfClient.GetGuidFirst(
                $"Document_ДоговорКредитаИЗайма?$filter=Number eq '{splitLoanAgreement[0]}' and ВидДоговора eq '{splitLoanAgreement[1]}'");

            if (handler.Model.LoanAgreement is null)
                handlerResult.ErrorWasOccured(new ValidationException("Договор кредита/займа не найден"));
            
            return handlerResult;
        },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указан договор кредита/займа: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });



    public static async Task<Result<DataForRequest, ValidationException>> HandleEmployeeAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitEmployee = ValidateModelAndInput(handler.PostFromBotModel.Employee, 1);
        return await splitEmployee.Match(async splitValidEmployee =>
        {
            handler.Model.Employee = await handler.UnfClient.GetGuidFirst($"Catalog_Сотрудники?$filter=Description eq '{splitValidEmployee[0]}'");

            if (handler.Model.Employee is null)
                handlerResult.ErrorWasOccured(new ValidationException("Сотрудник не найден"));
            
            return handlerResult;
        },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указан сотрудник: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleCurrencyAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitCurrency = ValidateModelAndInput(handler.PostFromBotModel.Currency, 2);
        return await splitCurrency.Match(async splitCurrencyArray =>
            {
                handler.Model.Currency = await handler.UnfClient.GetGuidFirst(
                    $"Catalog_Валюты?$filter=СимвольноеПредставление eq '{splitCurrencyArray[0]}' and Description eq '{splitCurrencyArray[1]}'");

                if (handler.Model.Currency is null)
                {
                    handlerResult.ErrorWasOccured(new ValidationException("Валюта не найдена"));
                    return handlerResult ;
                }

                handler.Model.ExchangeRate = handler.DefaultExchangeRate;
                handler.Model.Multiplicity = handler.DefaultMultiplicity;
                handler.Model.AmountCount = handler.Model.Amount * handler.Model.Multiplicity * handler.Model.ExchangeRate;
                
                return handlerResult;
            },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно введена валюта: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleOrganisationAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitOrganisation = ValidateModelAndInput(handler.PostFromBotModel.Organisation, 1);
        return await splitOrganisation.Match(async splitValidOrganisation =>
            {
                handler.Model.Organisation = await handler.UnfClient.GetGuidFirst(
                    $"Catalog_Организации?$filter=Description eq '{splitValidOrganisation[0]}'");

                if (handler.Model.Organisation is null)
                    handlerResult.ErrorWasOccured(new ValidationException("Организация не найдена"));
                
                return handlerResult;
            },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указана организация: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleOrganisationAccountAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitBankAccount = ValidateModelAndInput(handler.PostFromBotModel.OrganisationAccount, 2);
        return await splitBankAccount.Match(async splitBankAccountArray =>
        {
            handler.Model.OrganisationAccount = await handler.UnfClient.GetGuidFirst(
                $"Catalog_БанковскиеСчета?$filter=Code eq '{splitBankAccountArray[0]}' and Description eq '{splitBankAccountArray[1]}'");

            if (handler.Model.OrganisationAccount is null)
                handlerResult.ErrorWasOccured(new ValidationException("Банковский счет не найден"));

            return handlerResult;
        },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указан банковский счет: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleContractAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitContract = ValidateModelAndInput(handler.PostFromBotModel.Contract, 4);
        return await splitContract.Match(async splitContractArray =>
        {
            splitContractArray[3] = splitContractArray[3] == "Касса" ? "ИзКассы" : "СоСчета";
            handler.Model.Contract = await handler.UnfClient.GetGuidFirst(
                $"Document_Расход{splitContractArray[3]}?$filter=Number eq '{splitContractArray[0]}' and СуммаДокумента eq {splitContractArray[2]}");

            if (handler.Model.Contract is null)
            {
                handlerResult.ErrorWasOccured(new ValidationException("Договор не найден"));
                return handlerResult;
            }

            handler.Model.ContractType = "StandardODATA.Document_РасходИзКассы";
            
            return handlerResult;
        },
        failure =>
        {
            handlerResult.ErrorWasOccured(new ValidationException($"Неверно указан договор: \n{failure.Message}"));
            return Task.FromResult(handlerResult );
        });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleVatRateAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitVatRate = ValidateModelAndInput(handler.PostFromBotModel.VatRate, 2);
        return await splitVatRate.Match(async splitVatRateArray =>
        {
            handler.Model.Decryption ??= new DecryptionPayment[] { new() { LineNumber = "1" } };
            handler.Model.Decryption[0].VatRate = await handler.UnfClient.GetGuidFirst(
                $"Catalog_СтавкиНДС?$filter=ВидСтавкиНДС eq '{splitVatRateArray[0]}' and Description eq '{splitVatRateArray[1]}'");

            if (handler.Model.Decryption[0].VatRate is null)
                handlerResult.ErrorWasOccured(new ValidationException("Не найдена ставка НДС"));
            
            return handlerResult;
        },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указана ставка НДС: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleTaxTypeAsync(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler =>
    {
        var splitTaxType = ValidateModelAndInput(handler.PostFromBotModel.TypeOfTax, 2);
        return await splitTaxType.Match(async splitTaxTypeArray =>
            {
                handler.Model.TypeOfTax = await handler.UnfClient.GetGuidFirst(
                    $"Catalog_ВидыНалогов?$filter=Code eq '{splitTaxTypeArray[0]}' and Description eq '{splitTaxTypeArray[1]}'");

                if (handler.Model.TypeOfTax is null)
                    handlerResult.ErrorWasOccured(new ValidationException("Вид налога не найден"));
                
                return handlerResult;
            },
            failure =>
            {
                handlerResult.ErrorWasOccured(new ValidationException($"Неверно указан тип налога: \n{failure.Message}"));
                return Task.FromResult(handlerResult);
            });
    });

    public static async Task<Result<DataForRequest, ValidationException>> HandleAmountType(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler => await Task.Run(() =>
    {
        if (!StaticStructures.AmountTypes.Contains(handler.PostFromBotModel.AmountType!))
        {
            handlerResult.ErrorWasOccured(new ValidationException("Не найден тип суммы"));
            return handlerResult;
        }

        handler.Model.Decryption ??= new DecryptionPayment[] { new() { LineNumber = "1" } };
        handler.Model.Decryption[0].AmountType = handler.PostFromBotModel.AmountType;
        handler.Model.Decryption[0].AmountPayment = handler.Model.Amount;
        handler.Model.Decryption[0].AmountCount = handler.Model.Amount;
        
        return handlerResult;
    }));
    
    public static async Task<Result<DataForRequest, ValidationException>> HandlePaymentDeadline(
        this Result<DataForRequest, ValidationException> handlerResult) => await Matcher(handlerResult, async handler => await Task.Run(() =>
    {
        if (!DateTime.TryParse(handler.PostFromBotModel.PaymentDeadline, new CultureInfo("de-DE"), out var date))
        {
            handlerResult.ErrorWasOccured(new ValidationException("Дата введена неверно"));
        }
        handler.Model.PaymentDeadline = date;
        return handlerResult;
    }));

    private static Result<string[], ValidationException> ValidateModelAndInput(string? entity, int countSlices)
    {
        var splitEntity = entity!.Split('*');
        return splitEntity.Length != countSlices
            ? new ValidationException("Неверное количество полей в сущности")
            : splitEntity;
    }
    
    private static async Task<Result<DataForRequest, ValidationException>> Matcher(
        Result<DataForRequest, ValidationException> handlerResult, Func<DataForRequest, Task<Result<DataForRequest, ValidationException>>> success) =>
        await handlerResult.Match<Task<Result<DataForRequest, ValidationException>>>(
             success, 
             failure =>
             {
                 handlerResult.ErrorWasOccured(failure);
                 return Task.FromResult(handlerResult);
             });
}