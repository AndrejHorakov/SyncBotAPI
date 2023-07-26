namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromUnf<T>
{
    public EasyToStringList<T> Value { get; set; } = null!;

    public override string ToString()
    {
        return Value.ToString();
    }
}