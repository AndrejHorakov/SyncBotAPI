
using System.Net;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IReceiptRequestHandler _requestHandler;

    public HomeController(IUNFClient unfClient, IReceiptRequestHandler requestHandler)
    {
        _unfClient = unfClient;
        _requestHandler = requestHandler;
    }
    
    [HttpGet]
    [Route("ListItems")]
    public async Task<JsonResult> GetList(string? filter, IGetRequestHandler getRequestHandler)
    {
        return Json(await getRequestHandler.GetList(_unfClient, filter!));
    }

    [HttpPost]
    [Route("Income")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotReceiptModel postReceiptModel)
    {
        var res = new AnswerFromAPI();
        HttpResponseMessage? ans = null;
        var model = new PostReceiptToUNFModel();
        _requestHandler.HandleDefault(model, postReceiptModel.OperationType, postReceiptModel.Amount);
        switch (postReceiptModel.OperationType)
        {
            case "ОтПокупателя":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, postReceiptModel.Contragent!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!, postReceiptModel.ContractFromDecryptionOfPayment!);
                break;
            }
            case "Прочее":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                break;
            }
            case "ОтПоставщика":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, postReceiptModel.Contragent!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!, postReceiptModel.ContractFromDecryptionOfPayment!);
                await _requestHandler.HandleDecryptionDocumentAsync(model, _unfClient,
                    postReceiptModel.DocumentFromDecryptionOfPayment!);
                break;
            }
            case "РасчетыПоКредитам":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, postReceiptModel.Contragent!);
                await _requestHandler.HandleLoanAgreementAsync(model, _unfClient, postReceiptModel.LoanAgreement!);
                break;
            }
            case "ВозвратЗаймаСотрудником":
            {
               
                await _requestHandler.HandleLoanAgreementAsync(model, _unfClient, postReceiptModel.LoanAgreement!);
                await _requestHandler.HandleEmployeeAsync(model, _unfClient, postReceiptModel.Employee!);
                _requestHandler.HandleAmountTypeAsync(model, postReceiptModel.AmountType!);
                break;
            }
            case "ПокупкаВалюты":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                await _requestHandler.HandleCurrencyAsync(model, _unfClient, postReceiptModel.Currency!);
                break;
            }
            case "ПолучениеНаличныхВБанке":
            {
                await _requestHandler.HandleOrganisationAccountAsync(model, _unfClient,
                    postReceiptModel.OrganisationAccount!);
                break;
            }
            case "ПрочиеРасчеты":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, postReceiptModel.Contragent!);
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!, postReceiptModel.ContractFromDecryptionOfPayment!);
                break;
            }
            case "ОтПодотчетника":
            {
                await _requestHandler.HandleEmployeeAsync(model, _unfClient, postReceiptModel.Employee!);
                await _requestHandler.HandleContractAsync(model, _unfClient, postReceiptModel.Contract!);
                break;
            }
            case "ЛичныеСредстваПредпринимателя":
            {
                await _requestHandler.HandleCorrespondenceAsync(model, _unfClient, postReceiptModel.Correspondence!);
                await _requestHandler.HandleOrganisationAsync(model, _unfClient, postReceiptModel.Organisation!);
                break;
            }
            case "ОтНашейОрганизации":
            {
                await _requestHandler.HandleContragentAsync(model, _unfClient, postReceiptModel.Contragent!);
                await _requestHandler.HandleDecryptionContractAsync(model, _unfClient, model.Contragent!, postReceiptModel.ContractFromDecryptionOfPayment!);
                break;
            }
            default:
            {
                ans = new HttpResponseMessage();
                ans.StatusCode = HttpStatusCode.NotFound;
                res = new()
                {
                    Answer = "Ошибка в названии операции, обратитесь к разработчикам"
                };
                break;
            }
        }
        if (ans is null)
            ans = await _unfClient.PostReceipt(model);
        if (res.Answer is null)
            res.Answer = ans!.StatusCode == HttpStatusCode.OK
                ? "Операция прошла успешно!"
                : "Операция была прервана, произошла ошибка!";
        return Json(res);
    }
}