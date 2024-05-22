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
            const string cacheKey = "all_powers";

            try
            {
                // Try to get the cached powers from Redis
                var cachedPowers = await _cache.GetStringAsync(cacheKey);
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
                await _cache.SetStringAsync(cacheKey, serializedPowers);

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
            return Ok(await _powerService.AddPower(powerModelView));
        }

        [HttpPut]
        [Time("PUT Power")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Update([FromBody] EditPowerModelView editPower)
        {
            try
            {
                var powers = await _powerService.Update(editPower);
                return Ok(powers);
            }
            catch (PowerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Time("DELETE Power of Id {id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Delete(int id)
        {
            try
            {
                var powers = await _powerService.Delete(id);
                return Ok(powers);
            }
            catch (PowerNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
