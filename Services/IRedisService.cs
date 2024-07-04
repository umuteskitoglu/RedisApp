namespace RedisApp.Services;

public interface IRedisCacheService
{
    Task CacheList<T>(List<T> cache, string prefix, TimeSpan? keyExpire = null);
    Task Cache<T>(T cache, string prefix, TimeSpan? keyExpire = null);
    Task<List<T>> GetItems<T>();
    Task<bool> DeleteKey(string key);
    Task<T> GetItem<T>(string key);
    Task<bool> UpdateItem<T>(T cache, string prefix, TimeSpan? keyExpire = null);

}
