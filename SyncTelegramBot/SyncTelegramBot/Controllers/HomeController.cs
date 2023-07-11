using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.HelpModels;
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
}