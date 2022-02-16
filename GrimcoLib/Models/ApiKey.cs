namespace GrimcoLib.Models;

public class ApiKey
{
    public ApiKey()
    {
    }

    public ApiKey(int apiKeyId, int userId, string key)
    {
        ApiKeyId = apiKeyId;
        UserId = userId;
        Key = key;
    }

    public int ApiKeyId { get; set; }
    public int UserId { get; set; }
    public string Key { get; set; }
}