namespace SyncTelegramBot.Models.Entities;

public class DescriptionEntity : GuidEntity
{
    public string Description { get; set; }
    public override string ToString()
    {
        return Description;
    }
}