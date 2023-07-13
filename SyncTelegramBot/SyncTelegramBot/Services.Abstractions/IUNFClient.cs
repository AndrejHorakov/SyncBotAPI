namespace SyncTelegramBot.Services.Abstractions;

public interface IUNFClient
{
    public Task<HttpResponseMessage> GetFromUNF(string filter);
}