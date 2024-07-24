namespace Docs.Modules.Common.Result;

public record Result(bool Success, string? Message)
{
    public static Result OK(string message = "") => new(true, message);
    public static Result<T> OK<T>(T data, string? message = "") => new(data, true, message);

    public static Result Error(string message, bool isLog = true)
    {
        if (isLog) LogError(message);
        return new Result(false, message);
    }

    public static Result<T> Error<T>(string message, bool isLog = true)
    {
        if (isLog) LogError(message);
        return new Result<T>(default, false, message);
    }
    static void LogError(string e) => Log.Logger.Error(e);
}

public record Result<T>(T? Data, bool Success, string? Message) : Result(Success, Message);

/*public class Result
{
    protected bool success { get; set; }
    protected string? message { get; set; }

    public static Result OK(string message) => new() { success = true, message = message };
    public static Result Error(string message) => new() { success = false, message = message };
}

public class Result<T> : Result
{
    T? data { get; set; }

    public static Result<T> OK<T>(T data, string? message) =>
        new() { data = data, success = true, message = message };
}*/