using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class ActCompletedWork
{
    [JsonPropertyName("Number")]
    public string? Number { get; set; }
    
    [JsonPropertyName("ИдентификаторПлатежа")]
    public string? IdPayment { get; set; }

    public override string ToString()
    {
        return $"{Number}#{IdPayment}#АктВыполненныхРабот";
    }
}