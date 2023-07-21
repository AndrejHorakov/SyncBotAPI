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
    
    [JsonPropertyName("НовыйМеханизмИнкассации")]
    public bool? Type { get; set; }
    
    public override string ToString()
    {
        return Type is not null 
            ? $"{Number}*{Employee}*{DocumentAmount}*Касса"
            : $"{Number}*{Employee}*{DocumentAmount}*Счет";
    }
}