using SyncTelegramBot.Models.PostModels;
using SyncTelegramBot.Models.PostToUNFModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Models.HelpModels;

public class Handler
{
    public IUnfClient UnfClient { get; }
    public PostToUnfModel Model { get; set; }
    public PostFromBotModel PostFromBotModel { get; }
    public double DefaultExchangeRate { get; set; }
    public double DefaultMultiplicity { get; set; }

    public Handler(IUnfClient unfClient, PostFromBotModel postFromBotModel, PostToUnfModel postToUnfModel,
        double defaultMultiplicity, double defaultExchangeRate)
    {
        UnfClient = unfClient;
        Model = postToUnfModel;
        PostFromBotModel = postFromBotModel;
        DefaultExchangeRate = defaultExchangeRate;
        DefaultMultiplicity = defaultMultiplicity;
    }
}