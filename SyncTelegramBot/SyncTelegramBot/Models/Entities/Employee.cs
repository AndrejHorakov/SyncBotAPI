namespace SyncTelegramBot.Models.Entities;

public record Employee(string Description)
{
    public override string ToString()
    {
        return Description;
    }
}