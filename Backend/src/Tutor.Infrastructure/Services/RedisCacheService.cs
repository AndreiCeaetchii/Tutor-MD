using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly DistributedCacheEntryOptions _defaultOptions;

    public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redisConnection)
    {
        _cache = cache;
        _redisConnection = redisConnection;
        _defaultOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
        };
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var cachedData = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedData))
            return default;

        return JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = expiry.HasValue 
            ? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry }
            : _defaultOptions;

        var serializedData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, serializedData, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var data = await _cache.GetStringAsync(key);
        return !string.IsNullOrEmpty(data);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
        var keys = server.Keys(pattern: $"*{pattern}*").ToArray();
        
        var database = _redisConnection.GetDatabase();
        foreach (var key in keys)
        {
            await database.KeyDeleteAsync(key);
        }
    }
}