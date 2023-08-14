using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class VatRate
{
    [JsonPropertyName("ВидСтавкиНДС")]
    public string VATRate { get; set; }
    
    [JsonPropertyName("Description")]
    public string Description { get; set; }

    public override string ToString()
    {
        return $"{VATRate}#{Description}";
    }
}