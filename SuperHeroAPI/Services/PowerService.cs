using SuperHeroAPI.ModelViews;
using SuperHeroAPI.Exceptions;

namespace SuperHeroAPI.Services
{
    public class PowerService
    {
        private readonly DataContext _context;

        public PowerService(DataContext context)
        {
            _context = context;
        }

        //GET all powers
        public async Task<List<Power>> GetAllPowersAsync()
        {
            return await _context.Powers.ToListAsync();
        }

        //GET specific power
        public async Task<Power?> GetPower(int id)
        {
            var power = await _context.Powers.FindAsync(id);
            return power;
        }

        //POST power
        public async Task<List<Power>> AddPower(PowerModelView pmv)
        {
            var power = new Power
            {
                Tag = pmv.Tag,
                Description = pmv.Description
            };

            _context.Powers.Add(power);

            //persist changes
            await _context.SaveChangesAsync();

            //refetch
            return await _context.Powers.ToListAsync();
        }

        //PUT power
        public async Task<List<Power>> Update(EditPowerModelView epmv)
        {
            var dbPower = await _context.Powers.FindAsync(epmv.Id);
            if (dbPower == null)
            {
                throw new PowerNotFoundException($"Power with id {epmv.Id} not found.");
            }

            dbPower.Tag = epmv.Tag;
            dbPower.Description = epmv.Description;

            //persist changes
            await _context.SaveChangesAsync();

            return await _context.Powers.ToListAsync();
        }

        //DELETE power
        public async Task<List<Power>> Delete(int id)
        {
            var power = await _context.Powers.FindAsync(id);
            if (power == null)
            {
                throw new PowerNotFoundException($"Power with id {id} not found.");
            }

            //persist changes
            _context.Powers.Remove(power);
            await _context.SaveChangesAsync();

            return await _context.Powers.ToListAsync();
        }
    }
}
