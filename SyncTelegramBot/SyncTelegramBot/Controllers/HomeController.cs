using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Controllers;

[ApiController]
[Route("[controller]")]
public partial class HomeController : Controller
{
    private IUNFClient _unfClient;
    private IGetRequestHandler _getRequestHandler;

    public HomeController(IUNFClient unfClient, IGetRequestHandler getRequestHandler)
    {
        _unfClient = unfClient;
        _getRequestHandler = getRequestHandler;
    }
    
    [HttpGet]
    public async Task<JsonResult> GetList(string filter)
    {
        return Json(await _getRequestHandler.GetList(_unfClient, filter));
    }

    [HttpPost]
    [Route("/Moving")]
    public async Task<JsonResult> SaveMoving()
    {
        var res = new AnswerFromAPI();
        HttpResponseMessage? ans = null;
        return Json("");
    }
}

public partial class HomeController
{
 
}