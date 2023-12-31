using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class CorrectImplementation : GuidEntity
{
    [JsonPropertyName("Number")]
    public string? Number { get; set; }
    
    [JsonPropertyName("ВидОперации")]
    public string? OperationType { get; set; }

    public override string ToString()
    {
        return $"{Number} {OperationType}";
    }
}