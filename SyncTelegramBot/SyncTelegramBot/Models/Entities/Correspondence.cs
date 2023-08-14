using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class Correspondence : CodeAndDescription
{
    [JsonPropertyName("ТипСчета")]
    public string AccountType { get; set; }
    
    public override string ToString()
    {
        return $"{Code}#{Description}#{AccountType}";
    }
}