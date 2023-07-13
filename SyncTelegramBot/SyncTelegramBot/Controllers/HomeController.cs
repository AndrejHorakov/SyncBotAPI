using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Controllers;

[ApiController]
[Route("[controller]")]
public partial class HomeController : Controller
{
    private IUNFClient _unfClient;

    public HomeController(IUNFClient unfClient)
    {
        _unfClient = unfClient;
    }
    
    [HttpGet]
    public async Task<JsonResult> GetList(string filter)
    {
        AnswerFromAPI res;
        var ans = await _unfClient.GetFromUNF(filter);
        var entityName = filter.Split(new[] { '/', '\\', '?' })[0];
        object? output;
        try
        {
            var entityType = types[entityName];
            output = await ans.Content.ReadFromJsonAsync(entityType);
            res = new() { Answer = output?.ToString()};
        }
        catch (KeyNotFoundException exception)
        {
            res = new()
            {
                Answer = "На данный момент для отображения этого списка неизвестны уникальные параменты"
            };
        }
        catch
        {
            res = new()
            {
                Answer = "Произошла непредвиденная ошибка при обработке запроса"
            };
        }
        
     
        return Json(res);
    }

    [HttpPost]
    [Route("Receipt")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotReceiptModel postReceiptModel)
    {
        AnswerFromAPI res;
        var model = new PostReceiptToUNFModel()
        {
            Amount = postReceiptModel.Amount,
            Date = DateTime.Now,
            OperationType = postReceiptModel.OperationType
        };
        switch (postReceiptModel.OperationType)
        {
            case "ОтПоставщика":
            {
                var parsedEntity = postReceiptModel.Contragent.Split(' ');
                model.Contragent = await _unfClient
                    .GetGiudFirst($"Catalog_Контрагенты?$filter=Description eq {parsedEntity[0]} and Code eq {parsedEntity[1]}");
                model.Decryption = new()
                {
                    LineNumber = "1",
                    Contract = await _unfClient.GetGiudFirst(
                        $"Catalog_ДоговорыКонтрагентов?$filter=Контрагент_Key eq '{model.Contragent}'"),
                    AmountCount = postReceiptModel.Amount,
                    AmountPayment = postReceiptModel.Amount
                };
                var ans = await _unfClient.PostReceipt(model);
                if (ans.StatusCode == HttpStatusCode.OK)
                    res = new()
                    {
                        Answer = "Операция прошла успешно!"
                    };
                else
                    res = new()
                    {
                        Answer = "Операция была прервана, произошла ошибка!"
                    };
                break;
            }
            case "РасчетыПоКредитам":
            case "ВозвратЗаймаСотрудником":
            case "ПокупкаВалюты":
            case "ПолучениеНаличныхВБанке":
            case "ПрочиеРасчеты":
            case "ОтПодотчетника":
            case "ЛичныеСредстваПредпринимателя":
            case "ОтНашейОрганизации":
            default:
            {
                res = new()
                {
                    Answer = "Ошибка в имени операции, обратитесь к разработчикам"
                };
                break;
            }
        }
        return Json(res);
    }
    
    

    // It will be use, if we use reflection and search, init properties in runtime 
    // private void GetProperties(List<string> listJsonNames, PostReceiptToUNFModel model)
    // {
    //     
    // }
}

public partial class HomeController
{
    private Dictionary<string, Type> types = new()
    {
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUNF<VATRate>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUNF<Employee>),
        ["Catalog_Организации"] = typeof(AnswerFromUNF<Organisation>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUNF<TypesOfTaxes>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUNF<Contragent>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUNF<BankAccount>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUNF<Correspondence>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUNF<ContragentContract>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUNF<LoanAgreement>)
    };
    
    //for reflection
    // private Dictionary<string, List<string>> properties = new()
    // {
    //     ["ОтПокупателя"] = new() { "" }
    // };
}