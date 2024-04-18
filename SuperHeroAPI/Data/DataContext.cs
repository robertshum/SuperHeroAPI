using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //Table name SuperHeros
        public DbSet<SuperHero> SuperHeros { get; set; }
        public DbSet<Power> Powers { get; set; }
        public DbSet<SuperHeroPower> SuperHeroPowers { get; set; }

        //Many to Many relationship between super heroes and powers
        //enforce ref. integrity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SuperHeroPower>()
                .HasKey(sp => new { sp.SuperHeroId, sp.PowerId });

        }
    }
}
