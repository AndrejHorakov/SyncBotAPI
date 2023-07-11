namespace SyncTelegramBot.Models.Entities;

public record VATRate(string СтавкиНДС, string Description)
{
    public override string ToString()
    {
        return $"{Description} {СтавкиНДС}";
    }
}