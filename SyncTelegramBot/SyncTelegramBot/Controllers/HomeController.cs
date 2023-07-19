using Microsoft.AspNetCore.Mvc;
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
    private readonly ReceiptRequestHandler _handler;

    public HomeController(IUNFClient unfClient, ReceiptRequestHandler handler)
    {
        _unfClient = unfClient;
        _handler = handler;
    }
    
    [HttpGet]

    [Route("ListItems")]
    public async Task<JsonResult> GetList([FromQuery]string? entity, [FromQuery]string? addOptions, GetRequestHandler getRequestHandler)
    {
        return Json(await getRequestHandler.GetList(_unfClient, entity!, addOptions!, _handler));
    }

    [HttpPost]
    [Route("Income")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotModel postModel, PostReceiveRequestHandler postHandler)
    {
        return Json(await postHandler.SaveReceipt(_unfClient, postModel, _handler));
    }
}