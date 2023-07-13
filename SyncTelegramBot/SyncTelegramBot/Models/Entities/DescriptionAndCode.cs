namespace SyncTelegramBot.Models.Entities;

public record DescriptionAndCode(string Code, string Description)
{
    public override string ToString()
    {
        return $"{Code} {Description}";
    }
}