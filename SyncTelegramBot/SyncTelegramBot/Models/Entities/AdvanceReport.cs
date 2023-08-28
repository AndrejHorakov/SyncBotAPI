using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class AdvanceReport : GuidEntity
{

    [JsonPropertyName("Number")]
    public string Number { get; init; }
    
    [JsonPropertyName("Date")]
    public DateTime Date { get; init; }
    
    [JsonPropertyName("СуммаДокумента")]
    public int Amount { get; init; }

    public override string ToString()
    {
        return $"{Number} {Date} {Amount}";
    }
}