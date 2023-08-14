using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class CodeAndDescription
{
    [JsonPropertyName("Code")]
    public string Code { get; set; }
    
    [JsonPropertyName("Description")]
    public string Description { get; set; }
    
    public override string ToString()
    {
        return $"{Code}#{Description}";
    }
}