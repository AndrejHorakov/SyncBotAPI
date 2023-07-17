using System.Text;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromAPI> GetList(IUNFClient unfClient, string? entity, string? addOptions)
    {
        AnswerFromAPI res;
        var filter = await ParseOptionsToRequestString(entity!, addOptions!, unfClient);
        var ans = await unfClient.GetFromUNF(filter);
        
        object? output;
        try
        {
            if (!StaticStructures.Types.ContainsKey(entity!))
                res = new()
                {
                    Answer = "На данный момент для отображения этого списка неизвестны уникальные параменты"
                };
            else
            {
                var entityType = StaticStructures.Types[entity!];
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

    private async Task<string> ParseOptionsToRequestString(string? entity, string? addOptions, IUNFClient unfClient)
    {
        var builder = new StringBuilder();
        var tuples = addOptions!
            .Split(';')
            .Select(t => t.Split('='));
        foreach (var tuple in tuples)
        {
            if (StaticStructures.HandleOptionKey.ContainsKey(tuple[0]))
                tuple[1] = $"guid'{await StaticStructures.HandleOptionKey[tuple[0]](unfClient, tuple[1])}'";
            if (!String.IsNullOrEmpty(tuple[1]))
                builder.Append($"{tuple[0]} eq {tuple[1]}");
        }

        return builder.ToString();
    }
}