using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //Table name SuperHeros
        public DbSet<SuperHero> SuperHeros { get; set; }
    }
}
