using ILogger = Serilog.ILogger;

namespace Docs.Config;

public  static class SerilogConfiguration
{
    public static ILogger SerilogConfig(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        return loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .CreateLogger();
    }
}