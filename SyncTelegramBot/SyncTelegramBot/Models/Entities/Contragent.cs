namespace SyncTelegramBot.Models.Entities;

public record Contragent(string Code, string Description)
{
    public override string ToString()
    {
        return $"{Description} {Code}";
    }
}
