namespace Mac.Modules.Common;

public static class StaticServiceProvider
{
    public static IServiceProvider Instance { get; private set; }

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        Instance = serviceProvider;
    }
}