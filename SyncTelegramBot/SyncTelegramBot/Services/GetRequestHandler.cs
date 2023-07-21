using System.Text;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services.Abstractions;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromAPI> GetList(IUNFClient unfClient, string? keyEntity, string? addOptions, PostRequestHandler handler)
    {
        var filter = await ParseOptionsToRequestString(addOptions!, unfClient, handler);
        if (keyEntity.Contains("?$filter="))
            filter = filter.Substring(9, filter.Length-9);
        return await GetAnswerAsString(unfClient, keyEntity, filter);
    }

    private async Task<AnswerFromAPI> GetAnswerAsString(IUNFClient unfClient, string? keyEntity, string? filter)
    {
        if (String.IsNullOrEmpty(keyEntity))
            return new()
            {
                Answer = "Сущность не задана"
            };
        if (!StaticStructures.ListEntities.ContainsKey(keyEntity!))
            return new()
            {
                Answer = "На данный момент для отображения этого списка неизвестны его сущности"
            };
        var builder = new StringBuilder();
        try
        {
           
            foreach (var entity in StaticStructures.ListEntities[keyEntity!])
            {
                if (!StaticStructures.Types.ContainsKey(entity!))
                {
                    return new()
                    {
                        Answer = $"На данный момент для отображения этого списка неизвестны уникальные параметры одной из сущностей ({entity})"
                    };
                }
                builder.Append(await GetListAsStringAsync(unfClient, filter!, entity));
            }
        }
        catch
        {
            return new ()
            {
                Answer = "Произошла непредвиденная ошибка при обработке запроса"
            };
        }

        return new (){ Answer = builder.ToString() };
    }

    private async Task<string> ParseOptionsToRequestString(string? addOptions, IUNFClient unfClient, PostRequestHandler handler)
    {
        var builder = new StringBuilder();
        
       
        if (!String.IsNullOrEmpty(addOptions))
        {
            var tuples = addOptions!
                .Split(';')
                .Select(t => t.Split('='));
            builder.Append("?$filter=");
            foreach (var tuple in tuples)
            {
                if (StaticStructures.HandleOptionKey.ContainsKey(tuple[0]))
                    tuple[1] = await StaticStructures.HandleOptionKey[tuple[0]](tuple[1], handler);
                if (!String.IsNullOrEmpty(tuple[1]))
                    builder.Append($"{tuple[0]} eq {tuple[1]} and ");
            }
            builder.Remove(builder.Length - 5, 5);
        }

        return builder.ToString();
    }

    private async Task<string?> GetListAsStringAsync(IUNFClient unfClient, string filter, string entity)
    {
        object? output;
        var ans = await unfClient.GetFromUNF(entity + filter);
        var entityType = StaticStructures.Types[entity];
        output = await ans.Content.ReadFromJsonAsync(entityType);
        return output?.ToString();
    }
}