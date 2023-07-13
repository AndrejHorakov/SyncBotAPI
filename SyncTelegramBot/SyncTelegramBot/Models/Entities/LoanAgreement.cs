using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class LoanAgreement
{
    [JsonPropertyName("Number")]
    public string? Number { get; set; }
    
    [JsonPropertyName("ВидДоговора")]
    public string? ContractType { get; set; }
    
    [JsonPropertyName("РазмерПлатежа")]
    public double? PaymentSize { get; set; }
    
    [JsonPropertyName("СуммаДокумента")]
    public double? DocumentAmount { get; set; }
    public override string ToString()
    {
        return $"{Number} {ContractType} {PaymentSize}/{DocumentAmount}";
    }
}