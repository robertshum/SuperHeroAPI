using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //Table name SuperHeroes
        public DbSet<SuperHero> SuperHeros { get; set; }
        public DbSet<Power> Powers { get; set; }
        public DbSet<SuperHeroPower> SuperHeroPowers { get; set; }

        //Many to Many relationship between super heroes and powers
        //enforce ref. integrity.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SuperHeroPower>()
                .HasKey(sp => new { sp.SuperHeroId, sp.PowerId });

            modelBuilder.Entity<SuperHeroPower>()
                .HasOne(sp => sp.SuperHero)
                .WithMany(s => s.SuperHeroPowers)
                .HasForeignKey(sp => sp.SuperHeroId);

            //Make sure the cascade is on the power side.
            modelBuilder.Entity<SuperHeroPower>()
                .HasOne(sp => sp.Power)
                .WithMany(p => p.SuperHeroPowers)
                .HasForeignKey(sp => sp.PowerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
