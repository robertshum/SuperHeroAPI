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
        private readonly DataContext _context;
        private readonly PowerService _powerService;

        public PowerController(PowerService powerService, DataContext context)
        {
             _context = context;
            _powerService = powerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Power>>> Get()
        {
            var powers = await _powerService.GetAllPowersAsync();
            return Ok(powers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Power>> Get(int id)
        {
            var power = await _powerService.GetPower(id);
            if (power == null)
            {
                return BadRequest("Power not found.");
            }
            return Ok(power);
        }

        [HttpPost]
        public async Task<ActionResult<List<Power>>> AddPower([FromBody] PowerModelView powerModelView)
        {
            return Ok(await _powerService.AddPower(powerModelView));
        }

        [HttpPut]
        public async Task<ActionResult<List<Power>>> Update([FromBody] EditPowerModelView editPower)
        {
            try
            {
                var powers = await _powerService.Update(editPower);
                return Ok(powers);
            } catch (PowerNotFoundException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
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
