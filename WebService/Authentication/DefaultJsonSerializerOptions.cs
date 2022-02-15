using System.Text.Json;

namespace WebService.Authentication;

public static class DefaultJsonSerializerOptions
{
    public static JsonSerializerOptions Options => new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IgnoreNullValues = true
    };
}