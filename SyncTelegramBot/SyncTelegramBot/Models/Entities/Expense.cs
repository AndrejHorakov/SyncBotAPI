using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class Expense
{
    [JsonPropertyName("Number")]
    public string Number { get; set; }

    [JsonPropertyName("СуммаДокумента")]
    public int DocumentAmount { get; set; } 
    
    [JsonPropertyName("Подотчетник")]
    public OnlyDescription Employee { get; set; }
    
    public override string ToString()
    {
        return $"{Number}*{Employee} - {DocumentAmount}";
    }
}