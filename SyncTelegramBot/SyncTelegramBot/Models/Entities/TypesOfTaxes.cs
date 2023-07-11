namespace SyncTelegramBot.Models.Entities;

public record TypesOfTaxes(string Code, string Description)
{
    public override string ToString()
    {
        return $"{Description} {Code}";
    }
}