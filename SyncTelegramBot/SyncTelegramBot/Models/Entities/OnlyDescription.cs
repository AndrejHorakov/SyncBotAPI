namespace SyncTelegramBot.Models.Entities;

public record OnlyDescription(string Description)
{
    public override string ToString()
    {
        return Description;
    }
}