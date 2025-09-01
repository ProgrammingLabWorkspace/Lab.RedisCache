namespace Lab.RedisCache.Core.Contracts.Services
{
    public interface ICacheService<T>
    {

        public Task<T> Get(string key);
        public Task Set(string key, T value);
    }
}
