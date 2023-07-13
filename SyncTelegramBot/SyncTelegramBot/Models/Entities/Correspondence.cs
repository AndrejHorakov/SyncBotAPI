namespace SyncTelegramBot.Models.Entities;

public record Correspondence(string Code, string Description, string ТипСчета)
{
    public override string ToString()
    {
        return $"{Code.Trim()} {Description} {ТипСчета}";
    }
}