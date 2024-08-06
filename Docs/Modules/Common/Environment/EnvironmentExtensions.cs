namespace Mac.Modules.Common;

public static class EnvironmentExtensions
{
    static IServiceProvider serviceProvider;
    static IHostEnvironment env;

    static EnvironmentExtensions()
    {
        serviceProvider = StaticServiceProvider.Instance;
        env = serviceProvider.GetRequiredService<IHostEnvironment>();
    }

    public static async Task WhenDevelopment(this Task task)
    {
        if (env.IsDevelopment())
        {
            await task;
        }
    }


    public static async Task<T> WhenDevelopment<T>(this Task<T> task)
    {
        if (env.IsDevelopment())
        {
            return await task;
        }

        return default(T);
    }


    public static async Task<T> WhenDevelopment<T>(this Task<T> task, IHostEnvironment environment)
    {
        if (env.IsDevelopment())
        {
            return await task;
        }

        return default(T);
    }


    public static async Task<T> WhenDevelopment<T>(this Func<Task<T>> func)
    {
        if (env.IsDevelopment())
        {
            return await func.Invoke();
        }

        return default(T);
    }


    public static async Task WhenGivenEnvironment(this Task task, string environment)
    {
        if (env.EnvironmentName == environment)
        {
            await task;
        }
    }
    

    public static void WhenGivenEnvironment<T>(this Func<T> func, string environment)
    {
        if (env.EnvironmentName == environment)
        {
            func.Invoke();
        }
    }
}