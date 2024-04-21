using SuperHeroAPI.ModelViews;

namespace SuperHeroAPI.Services
{
    public interface ISuperHeroService
    {
        Task<List<SuperHero>> GetAllSuperHeroesAsync();
        Task<SuperHero> Get(int id);
        Task<List<SuperHero>> AddHero(SuperHeroModelView heroView);
        Task<List<SuperHero>> UpdateHero(EditSuperHeroModelView requestHero);
        Task<List<SuperHero>> Delete(int id);

    }
}