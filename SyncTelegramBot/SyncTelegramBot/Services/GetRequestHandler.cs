using System.Text;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromAPI> GetList(IUNFClient unfClient, string? entity, string? addOptions, ReceiptRequestHandler handler)
    {
        var filter = await ParseOptionsToRequestString(entity!, addOptions!, unfClient, handler);
        return await GetAnswerAsString(unfClient, entity, filter);
    }

    private async Task<AnswerFromAPI> GetAnswerAsString(IUNFClient unfClient, string? entity, string? filter)
    {
        AnswerFromAPI res;
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
                var ans = await unfClient.GetFromUNF(filter!);
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

    private async Task<string> ParseOptionsToRequestString(string? entity, string? addOptions, IUNFClient unfClient, ReceiptRequestHandler handler)
    {
        var builder = new StringBuilder();
        
        builder.Append($"{entity}");
       
        if (!String.IsNullOrEmpty(addOptions))
        {
            var tuples = addOptions!
                .Split(';')
                .Select(t => t.Split('='));
            builder.Append("?$filter=");
            foreach (var tuple in tuples)
            {
                if (StaticStructures.HandleOptionKey.ContainsKey(tuple[0]))
                    tuple[1] = await StaticStructures.HandleOptionKey[tuple[0]](unfClient, tuple[1], handler);
                if (!String.IsNullOrEmpty(tuple[1]))
                    builder.Append($"{tuple[0]} eq {tuple[1]} and ");
            }
            builder.Remove(builder.Length - 5, 5);
        }

        return builder.ToString();
    }
}