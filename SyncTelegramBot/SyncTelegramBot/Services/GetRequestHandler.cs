using System.Text;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromApi> GetList(DataForRequest dataForRequest, string? keyEntity, string? addOptions)
    {
        var filter = await ParseOptionsToRequestString(addOptions!, dataForRequest);
        if (keyEntity!.Contains("?$filter="))
            filter = filter.Substring(9, filter.Length-9);
        return await GetAnswerAsString(dataForRequest, keyEntity, filter);
    }

    private async Task<AnswerFromApi> GetAnswerAsString(DataForRequest dataForRequest, string? keyEntity, string? filter)
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
                builder.Append(await GetListAsStringAsync(dataForRequest, filter!, entity));
            }
        }
        catch
        {
            return new ()
            {
                Answer = "Произошла непредвиденная ошибка при обработке запроса"
            };
        }

        var result = builder.ToString();
        return new (){ Answer = string.IsNullOrEmpty(result) ? "Список пуст, объекты не найдены, предлагаем вернуться к выбору команды" : result };
    }

    private static async Task<string> ParseOptionsToRequestString(string? addOptions, DataForRequest dataForRequest)
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
                var handlingResult = await func(dataForRequest, propertyValue);
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

    private static async Task<string?> GetListAsStringAsync(DataForRequest dataForRequest, string filter, string entity)
    {
        var ans = await dataForRequest.UnfClient.GetFromUnf(entity + filter);
        return await ans!.Match(async httpResponseMessage =>
        {
            var entityType = StaticStructures.Types[entity];
            var output = await httpResponseMessage.Content.ReadFromJsonAsync(entityType);
            return output?.ToString() ?? "Не удалось прочитать результат запроса";
        },
            failure => Task.FromResult(failure.Message));
        
    }
}