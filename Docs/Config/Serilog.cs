using ILogger = Serilog.ILogger;

namespace Docs.Config;

public static class SerilogConfiguration
{
    public static ILogger SerilogConfig(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        return loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.OpenTelemetry(x =>
            {
                x.Endpoint = "http://localhost:5341/ingest/otlp/v1/logs";
                x.Protocol = OtlpProtocol.HttpProtobuf;
                x.Headers = new Dictionary<string, string>
                {
                    ["X-Seq-ApiKey"] = "M0lkBD73BqTLArKpIDae"
                };
                x.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "DocsService"
                };
            })
            /*.Enrich.WithCorrelationId()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")*/
            .CreateLogger();
    }
}

