namespace SyncTelegramBot.Models.Entities;

public record DescriptionEntity(string Description)
{
    public override string ToString()
    {
        return Description;
    }
}