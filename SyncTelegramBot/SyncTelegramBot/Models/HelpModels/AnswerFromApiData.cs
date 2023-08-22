using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.HelpModels;

public class AnswerFromApiData
{
    
    public AnswerFromApiData(string guid, string entity)
    {
        Guid = guid;
        Entity = entity;
    }

    public AnswerFromApiData(){ }
    
    [JsonRequired]
    [JsonPropertyName("guid")]
    public string Guid { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("entity")]
    public string Entity { get; set; }
}