using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services;

public static class ListAnswerFromApiDataExtensions
{
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData<T>(
        EasyToListAnswerFromApiData<T> list, string document = "") where T : GuidEntity => list.ToListAnswerFromApiData(string.Empty);
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<ActCompletedWork> list, string document = "") => list.ToListAnswerFromApiData("_АктВыполненныхРабот");
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<BuyerOrder> list, string document = "") => list.ToListAnswerFromApiData("_ЗаказПокупателя");
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<CorrectImplementation> list, string document = "") => list.ToListAnswerFromApiData("_Корректировка");
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<Invoice> list, string document = "") => list.ToListAnswerFromApiData("_Накладная");
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<ReceiveInAccount> list, string document = "") => list.ToListAnswerFromApiData("_ПоступлениеНаСчет");

    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<AdvanceReport> list, string document = "") => list.ToListAnswerFromApiData("_АвансовыйОтчет");
    
    public static List<AnswerFromApiData> ToListAnswerFromApiData(
        EasyToListAnswerFromApiData<Expense> list, string document = "") => list is null 
        ? null 
        : list.Count == 0 
            ? new List<AnswerFromApiData>() 
            : list[0].Type switch
            {
                null => list.ToListAnswerFromApiData("_Счет"),
                _ => list.ToListAnswerFromApiData("_Касса")
            };
}