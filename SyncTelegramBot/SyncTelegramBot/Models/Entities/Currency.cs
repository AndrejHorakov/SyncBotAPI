using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class Currency : GuidEntity
{
    [JsonPropertyName("Description")]
    public string Description { get; set; }
    
    [JsonPropertyName("СимвольноеПредставление")]
    public string Sign { get; set; }
    public override string ToString()
    {
        return $"{Sign} {Description}";
    }
}