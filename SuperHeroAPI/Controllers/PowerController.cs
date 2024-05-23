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
    public class PowerController : ControllerBase
    {
        private readonly IPowerService _powerService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<PowerController> _logger;

        private readonly string CACHE_ALL_POWERS = "all_powers";

        public PowerController(IPowerService powerService, IDistributedCache cache, ILogger<PowerController> logger)
        {
            _powerService = powerService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [Time("GET all Powers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Get()
        {
            try
            {
                //Try to get the cached powers from Redis
                var cachedPowers = await _cache.GetStringAsync(CACHE_ALL_POWERS);
                if (!string.IsNullOrEmpty(cachedPowers))
                {
                    _logger.LogInformation("Returning cached powers.");
                    var cachedPowersJson = JsonSerializer.Deserialize<List<Power>>(cachedPowers);
                    return Ok(cachedPowersJson);
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

            //get from db
            var powers = await _powerService.GetAllPowersAsync();

            try
            {
                //...now we attempt cache it
                var serializedPowers = JsonSerializer.Serialize(powers);
                await _cache.SetStringAsync(CACHE_ALL_POWERS, serializedPowers);

            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing Redis.");
            }

            return Ok(powers);
        }

        [HttpGet("{id}")]
        [Time("GET Power of Id {id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Power>> Get(int id)
        {
            try
            {
                var power = await _powerService.GetPower(id);
                return Ok(power);
            }
            catch (PowerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Time("POST Power")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> AddPower([FromBody] PowerModelView powerModelView)
        {
            var powers = new List<Power>();

            try
            {
                powers = await _powerService.AddPower(powerModelView);

                //...now we attempt to cache it
                var serializedPowers = JsonSerializer.Serialize(powers);
                await _cache.SetStringAsync(CACHE_ALL_POWERS, serializedPowers);
                return Ok(powers);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                //return what we have without cache
                return Ok(powers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A system error has occured.");

                //return what we have without cache, potentially empty list
                return Ok(powers);
            }
        }

        [HttpPut]
        [Time("PUT Power")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Update([FromBody] EditPowerModelView editPower)
        {
            var powers = new List<Power>();
            try
            {
                powers = await _powerService.Update(editPower);

                //...now we attempt to cache it
                var serializedPowers = JsonSerializer.Serialize(powers);
                await _cache.SetStringAsync(CACHE_ALL_POWERS, serializedPowers);
                return Ok(powers); 
            }
            catch (PowerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                // Returning powers even if cache fails
                return Ok(powers); 
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing Redis.");

                // Returning powers even if cache fails
                return Ok(powers); 
            }
        }

        [HttpDelete("{id}")]
        [Time("DELETE Power of Id {id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Delete(int id)
        {
            var powers = new List<Power>();
            try
            {
                powers = await _powerService.Delete(id);

                //...now we attempt to cache it
                var serializedPowers = JsonSerializer.Serialize(powers);
                await _cache.SetStringAsync(CACHE_ALL_POWERS, serializedPowers);

                return Ok(powers);
            }
            catch (PowerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RedisConnectionException rcex)
            {
                _logger.LogError(rcex, "Redis server is down. Could not cache results.");

                // Returning powers even if cache fails
                return Ok(powers);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing Redis.");

                // Returning powers even if cache fails
                return Ok(powers);
            }
        }
    }
}
