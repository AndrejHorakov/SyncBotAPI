using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly IUnfClient _unfClient;
    private readonly RequestValues _requestValues;

    public HomeController(IUnfClient unfClient, IOptions<RequestValues> requestValues)
    {
        _unfClient = unfClient;
        _requestValues = requestValues.Value;
    }
    
    [HttpGet]
    [Route("ListItems")]

    public async Task<JsonResult> GetList([FromQuery]string? entity, [FromQuery]string? addOptions, GetRequestHandler getRequestHandler)
    {
        return Json(await getRequestHandler.GetList(InitializeHandler(new ()), entity, addOptions));
    }

    [HttpPost]
    [Route("Income")]
    public async Task<JsonResult> SaveReceipt([FromForm] PostFromBotModel postModel)
    {
        postModel.Type = PostType.Receive;
        return Json(await PostRequestsService.SaveReceipt(InitializeHandler(postModel)));
    }
    
    [HttpPost]
    [Route("Expense")]
    public async Task<JsonResult> SaveExpense([FromForm] PostFromBotModel postModel)
    {
        postModel.Type = PostType.Expense;
        return Json(await PostRequestsService.SaveExpense(InitializeHandler(postModel)));
    }
    
    [HttpPost]
    [Route("Move")]
    public async Task<JsonResult> SaveMove([FromForm] PostFromBotModel postModel)
    {
        postModel.Type = PostType.Move;
        return Json(await PostRequestsService.SaveMove(InitializeHandler(postModel)));
    }

    private DataForRequest InitializeHandler(PostFromBotModel postFromBotModel) =>
        new(_unfClient, postFromBotModel, new (), _requestValues.DefaultMultiplicity, _requestValues.DefaultExchangeRate);
}