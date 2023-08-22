using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class ReceiveInAccount : GuidEntity
{
    [JsonPropertyName("Number")]
    public string? Number { get; set; }
    
    [JsonPropertyName("СуммаДокумента")]
    public int DocumentAmount { get; set; } 
    
    [JsonPropertyName("Date")]
    public string? Date { get; set; }

    public override string ToString()
    {
        return $"{Number} {Date} {DocumentAmount}";
    }
}