using GrimcoLib.Models;

namespace WebService.Services;

public class InMemoryGetApiKeyQuery : IGetApiKeyQuery
{
    private readonly IDictionary<string, ApiKey> _apiKeys;

    public InMemoryGetApiKeyQuery()
    {
        var existingApiKeys = new List<ApiKey>
        {
            new(1, 1, "C5BFF7F0-B4DF-475E-A331-F737424F013C")
        };

        _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
    }

    public Task<ApiKey?> Execute(string providedApiKey)
    {
        _apiKeys.TryGetValue(providedApiKey, out var key);
        return Task.FromResult(key);
    }
}