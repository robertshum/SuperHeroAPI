using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private static List<SuperHero> heroes = new List<SuperHero>
        {
            new SuperHero {
                Id = 1,
                Name= "Spider Man!",
                FirstName="Peter",
                LastName="Parker",
                Place="New York City"
            }
        };

        [HttpGet]
        //Must return a specific type and not a interface
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(heroes);
        }
    }
}
