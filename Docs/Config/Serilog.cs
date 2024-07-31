using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace Docs.Config;

public  static class SerilogConfiguration
{
    public static ILogger SerilogConfig(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        return loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            /*.Enrich.WithCorrelationId()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")*/
            .Enrich.WithThreadName()
            
            
            .CreateLogger();
    }
}