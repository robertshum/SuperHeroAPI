using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Exceptions;
using SuperHeroAPI.ModelViews;
using SuperHeroAPI.Services;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly IPowerService _powerService;

        public PowerController(IPowerService powerService)
        {
            _powerService = powerService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> Get()
        {
            var powers = await _powerService.GetAllPowersAsync();
            return Ok(powers);
        }

        [HttpGet("{id}")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Power>>> AddPower([FromBody] PowerModelView powerModelView)
        {
            return Ok(await _powerService.AddPower(powerModelView));
        }

        [HttpPut]
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
