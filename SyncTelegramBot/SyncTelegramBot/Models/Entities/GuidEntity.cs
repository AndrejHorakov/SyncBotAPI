using System.Text.Json.Serialization;

namespace SyncTelegramBot.Models.Entities;

public class GuidEntity
{
    [JsonPropertyName("Ref_Key")]
    public string? Guid { get; set; }
}