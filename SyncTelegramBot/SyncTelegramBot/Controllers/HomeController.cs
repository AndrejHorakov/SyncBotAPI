using Microsoft.AspNetCore.Mvc;
using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly IUNFClient _unfClient;


    public HomeController(IUNFClient unfClient)
    {
        _unfClient = unfClient;
    }
    
    [HttpGet]
    [Route("ListItems")]

    public async Task<JsonResult> GetList([FromQuery]string? entity, [FromQuery]string? addOptions, GetRequestHandler getRequestHandler)
    {
        return Json(await getRequestHandler.GetList(_unfClient, entity, addOptions, _handler));
    }

    [HttpPost]
    [Route("Income")]
    public async Task<JsonResult> SaveReceipt([FromBody] PostFromBotModel postModel, PostRequestsService postService)
    {
        postModel.Type = PostType.Receive;
        return Json(await postService.SaveReceipt(_unfClient, postModel, _handler));
    }
    
    [HttpPost]
    [Route("Expense")]
    public async Task<JsonResult> SaveExpense([FromBody] PostFromBotModel postModel, PostRequestsService postService)
    {
        postModel.Type = PostType.Expense;
        return Json(await postService.SaveExpense(_unfClient, postModel, _handler));
    }
    
    [HttpPost]
    [Route("Move")]
    public async Task<JsonResult> SaveMove([FromBody] PostFromBotModel postModel, PostRequestsService postService)
    {
        postModel.Type = PostType.Move;
        return Json(await postService.SaveMove(_unfClient, postModel, _handler));

    }
}