namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromUNF<T> 
{
    public EasyToStringList<T> Value { get; set; }

    public override string ToString()
    {
        return Value.ToString();
    }
}