using GrimcoLib.Models;

namespace WebService.Services;

public interface IGetApiKeyQuery
{
    Task<ApiKey?> Execute(string providedApiKey);
}