using Lab.RedisCache.Core.Contracts.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Lab.RedisCache.Application.Services
{
    public class RedisCacheService<T> : ICacheService<T>
    {
        private readonly IConnectionMultiplexer _multiplexer;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _multiplexer = connectionMultiplexer;
        }

        /// <summary>
        /// Obtém um valor do cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T>Get(string key)
        {
            var db = _multiplexer.GetDatabase();          

            var result = await db.StringGetAsync(key);

            if (result.IsNullOrEmpty) return default(T);

            return JsonSerializer.Deserialize<T>(result);
        }

        /// <summary>
        /// Insere um valor no cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Set(string key, T value)
        {
            var db = _multiplexer.GetDatabase();

            var result = JsonSerializer.Serialize(value);

            await db.StringSetAsync(key, 
                result, 
                TimeSpan.FromHours(1), 
                When.Always, 
                CommandFlags.None);
        }
    }
}
