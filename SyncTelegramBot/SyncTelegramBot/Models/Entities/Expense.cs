using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class Expense : GuidEntity
{
    [JsonPropertyName("Number")]
    public string? Number { get; set; }

    [JsonPropertyName("СуммаДокумента")]
    public int DocumentAmount { get; set; } 
    
    [JsonPropertyName("Подотчетник")]
    public DescriptionEntity? Employee { get; set; }
  
    [JsonPropertyName("НовыйМеханизмИнкассации")]
    public bool? Type { get; set; }
    
    public override string ToString()
    {
        return $"{Number} {Employee} {DocumentAmount}";
    }
}