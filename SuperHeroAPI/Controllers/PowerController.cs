using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.ModelViews;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly DataContext _context;

        public PowerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Power>>> Get()
        {
            return Ok(await _context.Powers.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Power>> Get(int id)
        {
            var power = await _context.Powers.FindAsync(id);
            if (power == null)
            {
                return BadRequest("Power not found.");
            }
            return Ok(power);
        }

        [HttpPost]
        public async Task<ActionResult<List<Power>>> AddPower([FromBody] PowerModelView powerModelView)
        {
            var power = new Power
            {
                Tag = powerModelView.Tag,
                Description = powerModelView.Description
            };

            _context.Powers.Add(power);

            //persist changes
            await _context.SaveChangesAsync();

            //refetch
            return Ok(await _context.Powers.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Power>>> Update([FromBody] EditPowerModelView editPower)
        {
            var dbPower = await _context.Powers.FindAsync(editPower.Id);
            if (dbPower == null)
            {
                return BadRequest("Power not found.");
            }

            dbPower.Tag = editPower.Tag;
            dbPower.Description = editPower.Description;

            //persist changes
            await _context.SaveChangesAsync();

            return Ok(await _context.Powers.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Power>>> Delete(int id)
        {
            var power = await _context.Powers.FindAsync(id);
            if (power == null)
            {
                return BadRequest("Power not found.");
            }

            _context.Powers.Remove(power);
            await _context.SaveChangesAsync();

            return Ok(await _context.Powers.ToListAsync());
        }
    }
}
