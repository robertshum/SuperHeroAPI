using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        //Must return a specific type and not a interface
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _context.SuperHeros.ToListAsync());
        }

        [HttpGet("{id}")]
        //Must return a specific type and not a interface
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _context.SuperHeros.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }
            return Ok(hero);
        }

        [HttpPost]
        //assumes this comes from the body, so we don't need the [FromBody] in front of SuperHero
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _context.SuperHeros.Add(hero);

            //we need to persist these changes
            await _context.SaveChangesAsync();

            //we need to re-fetch these changes
            return Ok(await _context.SuperHeros.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero requestHero)
        {
            var dbHero = await _context.SuperHeros.FindAsync(requestHero.Id);
            if (dbHero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }

            dbHero.Name = requestHero.Name;
            dbHero.FirstName = requestHero.FirstName;
            dbHero.LastName = requestHero.LastName;
            dbHero.Place = requestHero.Place;

            //persist changes
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeros.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var hero = await _context.SuperHeros.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }

            _context.SuperHeros.Remove(hero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeros.ToListAsync());
        }
    }
}
