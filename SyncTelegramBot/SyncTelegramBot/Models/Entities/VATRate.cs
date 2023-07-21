using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class VATRate
{
    [JsonPropertyName("ВидСтавкиНДС")]
    public string VatRate { get; set; }
    
    [JsonPropertyName("Description")]
    public string Description { get; set; }

    public override string ToString()
    {
        return $"{VatRate}*{Description}";
    }
}