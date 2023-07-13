using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.HelpModels;

public class DecryptionPayment
{
    [JsonRequired]
    [JsonPropertyName("LineNumber")]
    public string LineNumber { get; set; }
    
    [JsonPropertyName("Договор_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Contract { get; set; }
    
    [JsonPropertyName("СуммаРасчетов")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AmountCount { get; set; }
    
    [JsonPropertyName("СуммаПлатежа")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AmountPayment { get; set; }
    
    [JsonPropertyName("Документ")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Document { get; set; }
    
    [JsonPropertyName("Документ_Type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DocumentType { get; set; }
}