namespace Docs.Modules.Common.Result;

public record Result(bool success, string? message)
{
    public static Result OK(string message="") => new(true, message);
    public static Result Error(string message) => new(false, message);
    public static Result<T> OK<T>(T data, string? message="") => new(data,true, message);
}

public record Result<T>(T? data,bool success, string? message) : Result(success, message);

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
