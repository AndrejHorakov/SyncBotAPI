using System.Text.Json.Serialization;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Models.PostToUNFModels;

public class PostToUNFModel
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
    public double Amount { get; set; }
    
    [JsonRequired]
    [JsonPropertyName("СуммаУчета")]
    public double? AmountCount { get; set; }

    [JsonPropertyName("Корреспонденция_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Correspondence { get; set; }

    [JsonPropertyName("ДоговорКредитаЗайма_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LoanAgreement { get; set; }

    [JsonPropertyName("Подотчетник_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Employee { get; set; }

    [JsonPropertyName("ВалютаДенежныхСредств_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Currency { get; set; }

    [JsonPropertyName("СчетОрганизации_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrganisationAccount { get; set; }

    [JsonPropertyName("Организация_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Organisation { get; set; }
    
    [JsonPropertyName("Курс")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ExchangeRate { get; set; }

    
    [JsonPropertyName("Кратность")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Multiplicity { get; set; }
    
    [JsonPropertyName("РасшифровкаПлатежа")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DecryptionPayment[]? Decryption { get; set; }

    [JsonPropertyName("ВидНалога_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TypeOfTax { get; set; }
    
    [JsonPropertyName("СчетКонтрагента_Key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContragentAccount { get; set; }
}