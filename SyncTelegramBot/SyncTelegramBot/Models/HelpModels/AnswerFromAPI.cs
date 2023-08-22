namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromApi
{
    public string? Answer { get; set; }
    public List<AnswerFromApiData> Data { get; init; }
    
    public static implicit operator AnswerFromApi(string result) => new (result);
    
    public static implicit operator AnswerFromApi(List<AnswerFromApiData> data) => new(){ Data = data };
    
    public AnswerFromApi() { }

    public AnswerFromApi(string answer)
    {
        Answer = answer;
    }

    public AnswerFromApi(List<AnswerFromApiData> data)
    {
        Data = data;
    }
}