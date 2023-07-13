using System.Net;
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
    private readonly IUNFClient _unfClient;
    private IReceiptRequestHandler _requestHandler;

    public HomeController(IUNFClient unfClient, IReceiptRequestHandler requestHandler)
    {
        _unfClient = unfClient;
        _requestHandler = requestHandler;
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
        var res = new AnswerFromAPI();
        var model = new PostReceiptToUNFModel();
        _requestHandler.HandleDefault(model, postReceiptModel.OperationType, postReceiptModel.Amount);
        switch (postReceiptModel.OperationType)
        {
            case "ОтПоставщика":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, model.Contragent!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!);
                var ans = await _unfClient.PostReceipt(model);
                res.Answer = ans!.StatusCode == HttpStatusCode.OK
                    ? "Операция прошла успешно!"
                    : "Операция была прервана, произошла ошибка!";
                break;
            }
            case "РасчетыПоКредитам":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                var ans = await _unfClient.PostReceipt(model);
                res.Answer = ans!.StatusCode == HttpStatusCode.OK
                    ? "Операция прошла успешно!"
                    : "Операция была прервана, произошла ошибка!";
                break;
            }
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