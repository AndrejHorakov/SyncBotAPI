using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.PostModels;

public class PostFromBotModel
{
    [JsonPropertyName("contragent")]
    public string? Contragent { get; init; }
    
    [JsonPropertyName("operationType")]
    public string OperationType { get; init; }
    
    [JsonPropertyName("contract")]
    public string? Contract { get; init; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
    
    [JsonPropertyName("correspondence")]
    public string? Correspondence { get; init; }
    
    [JsonPropertyName("loanAgreement")]
    public string? LoanAgreement { get; init; }
    
    [JsonPropertyName("employee")]
    public string? Employee { get; init; }
    
    [JsonPropertyName("amountType")]
    public string? AmountType { get; init; }
    
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
    
    [JsonPropertyName("organisationAccount")]
    public string? OrganisationAccount { get; init; }
    
    [JsonPropertyName("documentFromDecryptionOfPayment")]
    public string? DocumentFromDecryptionOfPayment { get; init; }
    
    [JsonPropertyName("contractFromDecryptionOfPayment")]
    public string? ContractFromDecryptionOfPayment { get; init; }
    
    [JsonPropertyName("organisation")]
    public string? Organisation { get; init; }
}