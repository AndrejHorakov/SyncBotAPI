namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromApi
{
    public string? Answer { get; set; }
    
    public AnswerFromApi() { }

    public AnswerFromApi(string answer)
    {
        Answer = answer;
    }
}