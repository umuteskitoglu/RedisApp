using Newtonsoft.Json;
using NRedisStack.RedisStackCommands;
using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace RedisApp.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _muxer;
        private readonly IDatabase _redis;
        private readonly RedisConnectionProvider _provider;

        public RedisCacheService(IConnectionMultiplexer muxer, RedisConnectionProvider provider)
        {
            _redis = muxer.GetDatabase();
            _muxer = muxer;
            _provider = provider;
        }

        public async Task CacheList<T>(List<T> cache, string prefix, TimeSpan? keyExpire = null)
        {
            if (_muxer.IsConnected)
            {
                var JSON = _redis.JSON();
                foreach (var item in cache)
                {
                    var idProperty = item.GetType().GetProperty("Id");
                    if (idProperty == null)
                    {
                        throw new InvalidOperationException("Cache işlemi yapabilmek için Id property'si bulunmalı " + item.GetType().Name);
                    }
                    var idValue = idProperty.GetValue(item)?.ToString();
                    if (idValue == null)
                    {
                        idValue = Guid.NewGuid().ToString();
                    }

                    string jsonString = JsonConvert.SerializeObject(item);
                    var key = $"{prefix}:{idValue}";
                    var sonuc = await JSON.SetAsync(key, "$", jsonString);
                    if (keyExpire.HasValue)
                        _redis.KeyExpire(key, keyExpire);
                }
            }
        }
        public async Task Cache<T>(T cache, string prefix, TimeSpan? keyExpire = null)
        {
            if (_muxer.IsConnected)
            {
                var JSON = _redis.JSON();
                var idProperty = cache.GetType().GetProperty("Id");
                if (idProperty == null)
                {
                    throw new InvalidOperationException("Cache işlemi yapabilmek için Id property'si bulunmalı " + cache.GetType().Name);
                }
                var idValue = idProperty.GetValue(cache)?.ToString();
                if (idValue == null)
                {
                    idValue = Guid.NewGuid().ToString();
                }

                string jsonString = JsonConvert.SerializeObject(cache);
                var key = $"{prefix}:{idValue}";
                await JSON.SetAsync(key, "$", jsonString);
                if (keyExpire.HasValue)
                    _redis.KeyExpire(key, keyExpire);
            }
        }

        public async Task<List<T>> GetItems<T>()
        {
            try
            {
                var cachedItems = _provider.RedisCollection<T>();
                return cachedItems.ToList();
            }
            catch
            {
                return new List<T>();
            }
        }
        public async Task<bool> DeleteKey(string key)
        {
            return await _redis.KeyDeleteAsync(key);
        }
        public async Task<T> GetItem<T>(string key)
        {
            return await _provider.RedisCollection<T>().FindByIdAsync(key);
        }
        public async Task<bool> UpdateItem<T>(T cache, string prefix, TimeSpan? keyExpire = null)
        {
            if (_muxer.IsConnected)
            {
                var JSON = _redis.JSON();
                var idProperty = cache.GetType().GetProperty("Id");
                if (idProperty == null)
                {
                    throw new InvalidOperationException("Cache işlemi yapabilmek için Id property'si bulunmalı " + cache.GetType().Name);
                }
                var idValue = idProperty.GetValue(cache)?.ToString();
                if (idValue == null)
                {
                    idValue = Guid.NewGuid().ToString();
                }

                string jsonString = JsonConvert.SerializeObject(cache);
                var key = $"{prefix}:{idValue}";
                await JSON.SetAsync(key, "$", jsonString);
                if (keyExpire.HasValue)
                    _redis.KeyExpire(key, keyExpire);
                return true;
            }
            return false;
        }
    }
}
