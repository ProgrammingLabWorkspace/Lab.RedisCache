using Lab.RedisCache.Application.Services;
using Lab.RedisCache.Core.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Lab.RedisCache.Application
{
    public static class ApplicationExtension
    {      
        public static void AddRedis(this IServiceCollection services, string redisEndpoint, string redisPassword)
        {
            services.AddSingleton<IConnectionMultiplexer>(x =>
            {
                return ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints = { redisEndpoint },
                    Password = redisPassword,
                    AbortOnConnectFail = false
                });
            });

            services.AddSingleton(typeof(ICacheService<>), typeof(RedisCacheService<>));
        }
    }
}
