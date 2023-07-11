namespace SyncTelegramBot.Models.Entities;

public record Organisation(string Description)
{
    public override string ToString()
    {
        return Description;
    }
}