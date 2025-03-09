using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZambaFarm.Models
{
    public class FarmContext : IdentityDbContext
    {
        public DbSet<Rabbit> Rabbits { get; set; }
        public DbSet<Pig> Pigs { get; set; }
        public DbSet<Cattle> Cattles { get; set; }
        public DbSet<Goat> Goats { get; set; }
        public DbSet <Turkey> Turkeys { get; set; }

        public FarmContext(DbContextOptions<FarmContext> options)
            : base(options)
        {
        }

        public FarmContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rabbit>()
                .HasMany(r => r.Offspring)
                .WithOne(r => r.Mother)
                .HasForeignKey(r => r.MotherId)
                .OnDelete(DeleteBehavior.NoAction); // Prevents cascade path issue

            modelBuilder.Entity<Pig>()
                .HasMany(p => p.Offspring)
                .WithOne(p => p.Mother)
                .HasForeignKey(p => p.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cattle>()
                .HasMany(c => c.Offspring)
                .WithOne(c => c.Mother)
                .HasForeignKey(c => c.MotherId)
                .OnDelete(DeleteBehavior.NoAction); // Fixes foreign key issue

            modelBuilder.Entity<Goat>()
                .HasMany(g => g.Offspring)
                .WithOne(g => g.Mother)
                .HasForeignKey(g => g.MotherId)
                .OnDelete(DeleteBehavior.NoAction); // Fixes foreign key issue

            modelBuilder.Entity<Turkey>()
                .HasMany(t => t.Offspring)
                .WithOne(t => t.Mother)
                .HasForeignKey(t => t.MotherId)
                .OnDelete(DeleteBehavior.NoAction); // Fixes foreign key issue
        }
    }

}
