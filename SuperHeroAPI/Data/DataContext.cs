using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //Table name SuperHeroes
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<Power> Powers { get; set; }
    }
}
