using System.Text;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromApi> GetList(Handler handler, string? keyEntity, string? addOptions)
    {
        var filter = await ParseOptionsToRequestString(addOptions!, handler);
        if (keyEntity!.Contains("?$filter="))
            filter = filter.Substring(9, filter.Length-9);
        return await GetAnswerAsString(handler, keyEntity, filter);
    }

    private async Task<AnswerFromApi> GetAnswerAsString(Handler handler, string? keyEntity, string? filter)
    {
        if (string.IsNullOrEmpty(keyEntity))
            return new()
            {
                Answer = "Сущность не задана"
            };
        if (!StaticStructures.ListEntities.ContainsKey(keyEntity))
            return new()
            {
                Answer = "На данный момент для отображения этого списка неизвестны его сущности"
            };
        var builder = new StringBuilder();
        try
        {
            foreach (var entity in StaticStructures.ListEntities[keyEntity])
            {
                if (!StaticStructures.Types.ContainsKey(entity))
                {
                    return new()
                    {
                        Answer = $"На данный момент для отображения этого списка неизвестны уникальные параметры одной из сущностей ({entity})"
                    };
                }
                builder.Append(await GetListAsStringAsync(handler, filter!, entity));
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

    private static async Task<string> ParseOptionsToRequestString(string? addOptions, Handler handler)
    {
        var builder = new StringBuilder();

        if (string.IsNullOrEmpty(addOptions)) return builder.ToString();
        
        var keyValueTuples = addOptions
            .Split(';')
            .Select(t =>
            {
                var p = t.Split('=');
                return Tuple.Create(p[0], p[1]);
            });
        builder.Append("?$filter=");
        foreach (var (propertyName, propertyValue) in keyValueTuples)
        {
            var endPropertyValue = propertyValue;
            if (StaticStructures.HandleOptionKey.TryGetValue(propertyName, out var func))
            {
                var handlingResult = await func(handler, propertyValue);
                if (handlingResult.IsError)
                    return null!;
                endPropertyValue = handlingResult.Value;
            }
                    
            if (!string.IsNullOrEmpty(propertyValue))
                builder.Append($"{propertyName} eq {endPropertyValue} and ");
        }
        builder.Remove(builder.Length - 5, 5);

        return builder.ToString();
    }

    private static async Task<string?> GetListAsStringAsync(Handler handler, string filter, string entity)
    {
        var ans = await handler.UnfClient.GetFromUnf(entity + filter);
        var entityType = StaticStructures.Types[entity];
        var output = await ans.Content.ReadFromJsonAsync(entityType);
        return output?.ToString();
    }
}