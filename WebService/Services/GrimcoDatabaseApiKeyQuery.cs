using GrimcoDatabase.Context;
using GrimcoLib.Models;

namespace WebService.Services;

public class GrimcoDatabaseApiKeyQuery : IGetApiKeyQuery
{
    public Task<ApiKey?> Execute(string providedApiKey)
    {
        using var db = new GrimcoDatabaseContext();
        return Task.FromResult(db.ApiKeys.FirstOrDefault(key => key.Key == providedApiKey));
    }
}