using SyncTelegramBot.Models.Entities;

namespace SyncTelegramBot.Models.HelpModels;

public class EasyToListAnswerFromApiData<T> : List<T> where T : GuidEntity
{
    public List<AnswerFromApiData> ToListAnswerFromApiData(string document = "") =>
        this.Select(obj => new AnswerFromApiData($"{obj.Guid!}{document}", obj.ToString()!.Replace("\"", "\\\"")[..Math.Min(obj.ToString()!.Length, 31)]))
            .ToList();
}