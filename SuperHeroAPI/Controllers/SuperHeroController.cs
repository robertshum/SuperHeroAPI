using MethodTimer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using SuperHeroAPI.Exceptions;
using SuperHeroAPI.ModelViews;
using SuperHeroAPI.Services;
using System.Text.Json;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroService _heroService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<SuperHeroController> _logger;

        private readonly string CACHE_ALL_HEROES = "all_heroes";

        public SuperHeroController(ISuperHeroService superHeroService, IDistributedCache cache, ILogger<SuperHeroController> logger)
        {
            _heroService = superHeroService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [Time("GET All Heroes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //Must return a specific type and not a interface
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            try
            {
                // Try to get the cached powers from Redis
                var cachedHeroes = await _cache.GetStringAsync(CACHE_ALL_HEROES);
                if (!string.IsNullOrEmpty(cachedHeroes))
                {
                    _logger.LogInformation("Returning cached heroes.");
                    var cachedHeroesJson = JsonSerializer.Deserialize<List<SuperHero>>(cachedHeroes);
                    return Ok(cachedHeroesJson);
                }
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Fetching data from database.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing Redis.");
            }

            var heroes = await _heroService.GetAllSuperHeroesAsync();

            try
            {
                //...now we attempt to cache it
                var serializedHeroes = JsonSerializer.Serialize(heroes);
                await _cache.SetStringAsync(CACHE_ALL_HEROES, serializedHeroes);

            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing Redis.");
            }

            return Ok(heroes);
        }

        [HttpGet("{id}")]
        [Time("GET Hero of Id {id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //Must return a specific type and not a interface
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            try
            {
                var hero = await _heroService.Get(id);
                return Ok(hero);
            }
            catch (SuperHeroNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Time("POST Hero")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody] SuperHeroModelView heroView)
        {
            var heroes = new List<SuperHero>();

            try
            {
                heroes = await _heroService.AddHero(heroView);

                //...now we attempt to cache it
                var serializedHeroes = JsonSerializer.Serialize(heroes);
                await _cache.SetStringAsync(CACHE_ALL_HEROES, serializedHeroes);
                return Ok(heroes);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                //return what we have without cache
                return Ok(heroes);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "A system error has occured.");

                //return what we have without cache, potentially empty list
                return Ok(heroes);
            }
        }

        [HttpPut]
        [Time("PUT Hero")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero([FromBody] EditSuperHeroModelView requestHero)
        {
            var heroes = new List<SuperHero>();

            try
            {
                heroes = await _heroService.UpdateHero(requestHero);

                //...now we attempt to cache it
                var serializedHeroes = JsonSerializer.Serialize(heroes);
                await _cache.SetStringAsync(CACHE_ALL_HEROES, serializedHeroes);
                return Ok(heroes);
            }
            catch (SuperHeroNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                //return what we have without cache
                return Ok(heroes);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "A system error has occured.");

                //return what we have without cache, potentially empty list
                return Ok(heroes);
            }
        }

        [HttpDelete("{id}")]
        [Time("DELETE Hero of Id {id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var heroes = new List<SuperHero>();

            try
            {
                heroes = await _heroService.Delete(id);

                //...now we attempt to cache it
                var serializedHeroes = JsonSerializer.Serialize(heroes);
                await _cache.SetStringAsync(CACHE_ALL_HEROES, serializedHeroes);
                return Ok(heroes);
            }
            catch (SuperHeroNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                //return what we have without cache
                return Ok(heroes);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "A system error has occured.");

                //return what we have without cache, potentially empty list
                return Ok(heroes);
            }
        }
    }
}
