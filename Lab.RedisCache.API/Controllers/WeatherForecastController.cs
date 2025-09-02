using Lab.RedisCache.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab.RedisCache.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ICacheService<IEnumerable<WeatherForecast>> _cacheService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICacheService<IEnumerable<WeatherForecast>> cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Obtém a cache utilizada para armazenar itens no cache.
        /// </summary>
        private string CacheKey
        {
            get
            {
                //var connectionId = HttpContext.Connection.Id;
                //var weatherForecastName = nameof(WeatherForecast);

                //return $"{connectionId}_{weatherForecastName}";

                var weatherForecastName = nameof(WeatherForecast);

                return weatherForecastName;
            }
        }

        /// <summary>
        /// Obtém as temperaturas. Se elas estiverem salvas no cache, utiliza os registros que estão no cache. Do contrário,
        /// recupera os valores da lista de temperaturas.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            // Obtém as temperaturas do cache
            var cachedResponse = await _cacheService.Get(CacheKey);

            // Se houver temperaturas registradas no cache...
            if (cachedResponse is not null && cachedResponse.Any())
            {
                // retorna as temperaturas cacheadas.
                return cachedResponse;
            }

            // Obtém as temperaturas da lista.
            var response = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            // Coloca as temperaturas no cache.
            // No próximo request, será obtido as temperaturas do cache.
            await _cacheService.Set(CacheKey, response);

            return response;
        }
    }
}
