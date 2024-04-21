using SuperHeroAPI.Exceptions;
using SuperHeroAPI.ModelViews;

namespace SuperHeroAPI.Services
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly DataContext _context;

        public SuperHeroService(DataContext context)
        {
            _context = context;
        }

        //GET all superheroes
        public async Task<List<SuperHero>> GetAllSuperHeroesAsync()
        {
            return await _context.SuperHeroes.Include(h => h.Powers).ToListAsync();
        }

        //GET specific superhero
        public async Task<SuperHero> Get(int id)
        {
            var hero = await _context.SuperHeroes
                .Where(hero => hero.Id == id)
                .Include(hero => hero.Powers)
                .FirstOrDefaultAsync();

            if (hero == null)
            {
                throw new SuperHeroNotFoundException($"Hero/Villan of id: {id} not found.");
            }

            return hero;
        }

        //POST add superhero
        public async Task<List<SuperHero>> AddHero(SuperHeroModelView heroView)
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
            return await _context.SuperHeroes.ToListAsync();
        }

        //PUT superhero
        public async Task<List<SuperHero>> UpdateHero(EditSuperHeroModelView requestHero)
        {
            var dbHero = await _context.SuperHeroes
                .Where(hero => hero.Id == requestHero.Id)
                .Include(hero => hero.Powers)
                .FirstOrDefaultAsync();

            if (dbHero == null)
            {
                throw new SuperHeroNotFoundException($"Hero/Villan of id: {requestHero.Id} not found.");
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

            return await _context.SuperHeroes.ToListAsync();
        }

        //DELETE superhero
        public async Task<List<SuperHero>> Delete(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                throw new SuperHeroNotFoundException($"Hero/Villan of id: {id} not found.");
            }

            _context.SuperHeroes.Remove(hero);
            await _context.SaveChangesAsync();

            //it's OK if we don't show the powers here.
            return await _context.SuperHeroes.ToListAsync();
        }
    }
}
