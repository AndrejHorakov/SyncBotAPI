using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.PostModels;

public class PostFromBotReceiptModel
{
    public string? Contragent { get; init; }
    
    public string? OperationType { get; init; }
    
    public string? Contract { get; init; }
    
    public int? Amount { get; init; }
    
    public string? Correspondence { get; init; }
    
    public string? Document { get; init; }
    
    public string? LoanAgreement { get; init; }
    
    public string? Employee { get; init; }
    
    public string? AmountType { get; init; }
    
    public string? Currency { get; init; }
    
    public string? OrganisationAccount { get; init; }
    
    public string? DocumentFromDecryptionOfPayment { get; init; }
    
    public string? Organisation { get; init; }
}