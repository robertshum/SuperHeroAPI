using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Exceptions;
using SuperHeroAPI.ModelViews;
using SuperHeroAPI.Services;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly SuperHeroService _heroService;

        public SuperHeroController(SuperHeroService superHeroService)
        {
            _heroService = superHeroService;
        }

        [HttpGet]
        //Must return a specific type and not a interface
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _heroService.GetAllSuperHeroesAsync());
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<List<SuperHero>>> AddHero([FromBody] SuperHeroModelView heroView)
        {
            return await _heroService.AddHero(heroView);
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero([FromBody] EditSuperHeroModelView requestHero)
        {
            try
            {
                var heroes = await _heroService.UpdateHero(requestHero);
                return Ok(heroes);
            }
            catch (SuperHeroNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            try
            {
                var heroes = await _heroService.Delete(id);
                return Ok(heroes);
            }
            catch (SuperHeroNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
