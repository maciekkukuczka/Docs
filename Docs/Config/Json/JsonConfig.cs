namespace Docs.Config.Json;

public static class JsonConfig
{
    public static JsonSerializerOptions SetJsonReferenceHandlerToPreserve()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true ,// opcjonalne, aby zwiększyć czytelność JSON
            PropertyNameCaseInsensitive = true
        };
        return options;
    }
}