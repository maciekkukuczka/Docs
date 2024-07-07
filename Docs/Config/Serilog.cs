using ILogger = Serilog.ILogger;

namespace Docs.Config;

public static class SerilogConfiguration
{
    public static ILogger SerilogConfig(this LoggerConfiguration loggerConfiguration)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        return loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .CreateLogger();
    }
}