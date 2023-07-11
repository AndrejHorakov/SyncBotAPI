namespace SyncTelegramBot.Models.Entities;

public record ContragentContract(string Code, string Description)
{
    public override string ToString()
    {
        return $"{Code} {Description}";
    }
}