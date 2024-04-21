using SuperHeroAPI.ModelViews;

namespace SuperHeroAPI.Services
{
    public interface IPowerService
    {
        Task<List<Power>> GetAllPowersAsync();
        Task<Power?> GetPower(int id);

        Task<List<Power>> AddPower(PowerModelView pmv);
        Task<List<Power>> Update(EditPowerModelView epmv);
        Task<List<Power>> Delete(int id);

    }
}
