using System.ComponentModel.DataAnnotations;
using System.Text;
using SyncTelegramBot.Models.Entities;
using SyncTelegramBot.Models.HelpModels;

namespace SyncTelegramBot.Services;

public class GetRequestHandler
{
    public async Task<AnswerFromApi> GetList(DataForRequest dataForRequest, string? keyEntity, string? addOptions)
    {
        var filter = await ParseOptionsToRequestString(addOptions!);//, dataForRequest);
        if (keyEntity!.Contains("?$filter="))
            filter = filter.Substring(9, filter.Length-9);
        return await GetAnswerFromApi(dataForRequest, keyEntity, filter);
    }

    private async Task<AnswerFromApi> GetAnswerFromApi(DataForRequest dataForRequest, string? keyEntity, string? filter)
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
        var data = new List<AnswerFromApiData>();
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

                var readResult = await GetAnswerFromUnfAsync(dataForRequest, filter!, entity);
                if (readResult!.IsError)
                    return readResult.Value!;
                data = data.Concat(readResult.Value!.Data).ToList();
            }
        }
        catch
        {
            return new ()
            {
                Answer = "Произошла непредвиденная ошибка при обработке запроса"
            };
        }

        if (data.Count == 0)
            return "Список пуст: объекты не найдены, предлагаем вернуться к выбору команды";
        return data;
    }

    private static Task<string> ParseOptionsToRequestString(string? addOptions)//, DataForRequest dataForRequest)
    {
        var builder = new StringBuilder();

        if (string.IsNullOrEmpty(addOptions)) return Task.FromResult(builder.ToString());
        
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
            // var endPropertyValue = propertyValue;
            // if (StaticStructures.HandleOptionKey.TryGetValue(propertyName, out var func))
            // {
            //     var handlingResult = await func(dataForRequest, propertyValue);
            //     if (handlingResult.IsError)
            //         return null!;
            //     endPropertyValue = handlingResult.Value;
            // }
                    
            if (!string.IsNullOrEmpty(propertyValue))
                builder.Append($"{propertyName} eq {propertyValue} and ");
        }
        builder.Remove(builder.Length - 5, 5); //Delete last "and"

        return Task.FromResult(builder.ToString());
    }

    private static async Task<Result<AnswerFromApi, ValidationException>?> GetAnswerFromUnfAsync(DataForRequest dataForRequest, string filter, string entity)
    {
        var ans = await dataForRequest.UnfClient.GetFromUnf(entity + filter);
        return await ans!.Match<Task<Result<AnswerFromApi, ValidationException>?>>(async httpResponseMessage =>
        {
            var entityType = StaticStructures.Types[entity];
            var content = await httpResponseMessage.Content.ReadFromJsonAsync(entityType);
            var output = content as dynamic;
            if (output is null)
                return await Task.FromResult<Result<AnswerFromApi, ValidationException>>(
                    new AnswerFromApi("Не удалось прочитать результат запроса"));
            return await Task.FromResult<Result<AnswerFromApi, ValidationException>>(new AnswerFromApi(ListAnswerFromApiDataExtensions.ToListAnswerFromApiData(output.Value)));
        },
            failure => Task.FromResult<Result<AnswerFromApi, ValidationException>?>(new AnswerFromApi(failure.Message)));
    }
}