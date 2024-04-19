using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.ModelViews;

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
            return Ok(await _context.SuperHeroes.Include(h => h.Powers).ToListAsync());
        }

        [HttpGet("{id}")]
        //Must return a specific type and not a interface
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _context.SuperHeroes
                .Where(hero => hero.Id == id)
                .Include(hero => hero.Powers)
                .FirstOrDefaultAsync();

            if (hero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody] SuperHeroModelView heroView)
        {
            //base fields for hero
            var newHero = new SuperHero
            {
                Name = heroView.Name,
                FirstName = heroView.FirstName,
                LastName = heroView.LastName,
                Description = heroView.Description,
                Place = heroView.Place,
            };

            //get the powers associated with the hero
            var powers = await _context.Powers.Where(p => heroView.PowerIds.Contains(p.Id)).ToListAsync();

            //add the powers!
            foreach (var power in powers)
            {
                newHero.Powers.Add(power);
            }

            _context.SuperHeroes.Add(newHero);

            //we need to persist these changes
            await _context.SaveChangesAsync();

            //we need to re-fetch these changes
            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero([FromBody] EditSuperHeroModelView requestHero)
        {
            var dbHero = await _context.SuperHeroes
                .Where(hero => hero.Id == requestHero.Id)
                .Include(hero => hero.Powers)
                .FirstOrDefaultAsync();

            if (dbHero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }

            //get the 'new' powers associated with the hero
            var powers = await _context.Powers.Where(p => requestHero.PowerIds.Contains(p.Id)).ToListAsync();

            //Powers to add
            var newPowersToAdd = new List<Power>();
            foreach (var power in powers)
            {
                if (!dbHero.Powers.Any(p => p.Id == power.Id))
                {
                    newPowersToAdd.Add(power);
                }
            }

            //Powers to remove
            var powersToRemove = dbHero.Powers
                .Where(dbhp => !powers.Any(p => p.Id == dbhp.Id))
                .ToList();

            foreach (var superHeroPowerToRemove in powersToRemove)
            {
                dbHero.Powers.Remove(superHeroPowerToRemove);
            }

            //Add powers to dbHero
            foreach (var powerToAdd in newPowersToAdd)
            {
                dbHero.Powers.Add(powerToAdd);
            }

            dbHero.Name = requestHero.Name;
            dbHero.FirstName = requestHero.FirstName;
            dbHero.LastName = requestHero.LastName;
            dbHero.Place = requestHero.Place;
            dbHero.Description = requestHero.Description;

            //persist changes
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero/Villan not found.");
            }

            _context.SuperHeroes.Remove(hero);
            await _context.SaveChangesAsync();

            //it's OK if we don't show the powers here.
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
