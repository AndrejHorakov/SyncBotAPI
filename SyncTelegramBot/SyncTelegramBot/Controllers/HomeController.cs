
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
        return Json(await _getRequestHandler.GetList(_unfClient, filter));
    }

    [HttpPost]
    [Route("Receipt")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotReceiptModel postReceiptModel)
    {
        var res = new AnswerFromAPI();
        HttpResponseMessage? ans;
        var model = new PostReceiptToUNFModel();
        _requestHandler.HandleDefault(model, postReceiptModel.OperationType, postReceiptModel.Amount);
        switch (postReceiptModel.OperationType)
        {
            case "ОтПоставщика":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, model.Contragent!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!);
                ans = await _unfClient.PostReceipt(model);
                break;
            }
            case "РасчетыПоКредитам":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                ans = await _unfClient.PostReceipt(model);
                break;
            }
            case "ВозвратЗаймаСотрудником":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                ans = await _unfClient.PostReceipt(model);
                break;
            }
            case "ПокупкаВалюты":
            case "ПолучениеНаличныхВБанке":
            case "ПрочиеРасчеты":
            case "ОтПодотчетника":
            case "ЛичныеСредстваПредпринимателя":
            case "ОтНашейОрганизации":
            default:
            {
                ans = new HttpResponseMessage();
                res = new()
                {
                    Answer = "Ошибка в имени операции, обратитесь к разработчикам"
                };
                break;
            }
        }
        if (res.Answer is null)
            res.Answer = ans!.StatusCode == HttpStatusCode.OK
                ? "Операция прошла успешно!"
                : "Операция была прервана, произошла ошибка!";
        return Json(res);
    }
    
    

    // It will be use, if we use reflection and search, init properties in runtime 
    // private void GetProperties(List<string> listJsonNames, PostReceiptToUNFModel model)
    // {
    //     
    // }
}