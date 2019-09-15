using Microsoft.EntityFrameworkCore;

namespace Kash.Elector.Data
{
    public class ElectorContext : DbContext
    {
        public ElectorContext(DbContextOptions<ElectorContext> options) : base(options) { }

        public DbSet<Election> Elections { get; set; }

        public DbSet<Elector> Electors { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<ElectoralList> ElectoralLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ElectoralList>()
                .HasMany(p => p.Districts);

            modelBuilder.Entity<District>()
                .HasMany(p => p.ElectoralLists);
        }
    }
}
