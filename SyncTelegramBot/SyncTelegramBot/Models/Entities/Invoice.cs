using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class Invoice : GuidEntity
{
    [JsonPropertyName("Number")]
    public string Number { get; set; }
    
    [JsonPropertyName("ВидОперации")]
    public string OperationType { get; set; }
    
    [JsonPropertyName("СуммаДокумента")]
    public int DocumentAmount { get; set; }
    
    [JsonPropertyName("Автор")]
    public DescriptionEntity Author { get; set; }
        
    public override string ToString()
    {
        return $"{Number} {OperationType} {DocumentAmount} {Author}";
    }
}