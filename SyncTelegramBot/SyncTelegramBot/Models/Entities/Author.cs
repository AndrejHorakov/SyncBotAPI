namespace SyncTelegramBot.Models.Entities;

public record Автор(string Description)
{
    public override string ToString()
    {
        return Description;
    }
}