namespace SyncTelegramBot.Models.Entities;

public record BankAccount(string Owner, string Description)
{
    public override string ToString()
    {
        return $"{Owner} {Description}";
    }
}