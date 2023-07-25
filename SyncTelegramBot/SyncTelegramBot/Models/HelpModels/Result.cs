namespace SyncTelegramBot.Models.HelpModels;

public class Result<TValue, TError>
{
    public TValue? Value { get; private set; }
    public TError? Error { get; private set; }
    
    public bool IsError { get; private set; }
    public bool IsSuccess => !IsError;
    
    private Result(TValue value)
    {
        IsError = false;
        Value = value;
        Error = default;
    }
    
    private Result(TError error)
    {
        IsError = true;
        Error = error;
        Value = default; 
    }

    public void ErrorWasOccured(TError error)
    {
        IsError = true;
        Error = error;
    }

    public void ChangeValue(TValue value) => Value = value;

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
        !IsError ? success(Value!) : failure(Error!);
}