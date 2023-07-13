using System.Text.Json.Serialization;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Models.PostToUNFModels;

public class PostReceiptToUNFModel
{
    [JsonRequired]
    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Contragent { get; set; }

    [JsonRequired]
    [JsonPropertyName("ВидОперации")]
    public string OperationType { get; set; }

    [JsonPropertyName("Документ")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Contract { get; set; }

    [JsonPropertyName("Документ_Type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContractType { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("СуммаДокумента")]
    public int Amount { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Correspondence { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Document { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LoanAgreement { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Employee { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AmountType { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Currency { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrganisationAccount { get; set; }

    [JsonPropertyName("Контрагент_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Organisation { get; set; }
    
    [JsonPropertyName("РасшифровкаПлатежа")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DecryptionPayment? Decryption { get; set; }
}