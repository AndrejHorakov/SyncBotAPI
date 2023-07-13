using Microsoft.AspNetCore.Mvc;
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
}

public partial class HomeController
{
 
}