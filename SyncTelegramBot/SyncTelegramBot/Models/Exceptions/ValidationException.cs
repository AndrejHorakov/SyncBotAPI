namespace SyncTelegramBot.Models.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string msg) : base(msg) { }
}