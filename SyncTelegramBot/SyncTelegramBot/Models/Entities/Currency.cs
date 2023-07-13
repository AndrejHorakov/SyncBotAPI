namespace SyncTelegramBot.Models.Entities;

public record Currency(string Description, string СимвольноеПредставление)
{
    public override string ToString()
    {
        return $"{СимвольноеПредставление}, {Description}";
    }
}