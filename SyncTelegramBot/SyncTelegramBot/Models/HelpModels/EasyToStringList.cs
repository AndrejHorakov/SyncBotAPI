using System.Text;

namespace SyncTelegramBot.Models.HelpModels;

public class EasyToStringList<T> : List<T>
{
    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var obj in this)
        {
            builder.Append(obj);
            builder.Append("\n");
        }

        return builder.ToString();
    }
}