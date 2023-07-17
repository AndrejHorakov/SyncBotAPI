using System.Formats.Tar;

namespace SyncTelegramBot.Models.HelpModels;

public class RequestValues
{
    public const string Position = "RequestStrings";
        
    public string BaseUri { get; set; }
    
    public string Authorization { get; set; }

    public double DefaultExchangeRate { get; set; }
    
    public double DefaultMultiplicity { get; set; }
}