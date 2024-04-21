using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //used for mocking
        public DataContext() : base() { }

        //Table name SuperHeroes and Powers
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<Power> Powers { get; set; }
    }
}
