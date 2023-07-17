using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromAPI> GetList(IUNFClient unfClient, string filter)
    {
        AnswerFromAPI res;
        var ans = await unfClient.GetFromUNF(filter);
        var entityName = filter.Split(new[] { '/', '\\', '?' })[0];
        
        object? output;
        try
        {
            if (!StaticStructures.Types.ContainsKey(entityName))
                res = new()
                {
                    Answer = "На данный момент для отображения этого списка неизвестны уникальные параменты"
                };
            else
            {
                var entityType = StaticStructures.Types[entityName];
                output = await ans.Content.ReadFromJsonAsync(entityType);
                res = new() { Answer = output?.ToString()};
            }
        }
        catch
        {
            res = new()
            {
                Answer = "Произошла непредвиденная ошибка при обработке запроса"
            };
        }

        return res;
    }
}