
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : Controller
{

    private readonly IUNFClient _unfClient;
    private IReceiptRequestHandler _requestHandler;

    public HomeController(IUNFClient unfClient, IReceiptRequestHandler requestHandler)
    {
        _unfClient = unfClient;
        _requestHandler = requestHandler;
    }
    
    [HttpGet]
    public async Task<JsonResult> GetList(string? filter, GetRequestHandler getRequestHandler)
    {
        return Json(await getRequestHandler.GetList(_unfClient, filter!));
    }

    [HttpPost]
    [Route("Income")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotModel postModel, PostReceiveRequestHandler postHandler)
    {
        return Json(await postHandler.SaveReceipt(_unfClient, postModel));
    }
}