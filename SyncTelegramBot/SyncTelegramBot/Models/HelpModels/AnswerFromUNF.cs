using SyncTelegramBot.Models.Entities;

namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromUnf<T> where T : GuidEntity
{
    public EasyToListAnswerFromApiData<T> Value { get; set; } = null!;

    public List<AnswerFromApiData> ToListAnswerFromApiData()
    {
        return Value.ToListAnswerFromApiData();
    }
}