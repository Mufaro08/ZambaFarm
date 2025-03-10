using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ZambaFarm.Models
{
    public class FarmContext : IdentityDbContext
    {
        public DbSet<Rabbit> Rabbits { get; set; }
        public DbSet<Pig> Pigs { get; set; }
        public DbSet<Cattle> Cattles { get; set; }
        public DbSet<Goat> Goats { get; set; }
        public DbSet<Turkey> Turkeys { get; set; }

        public FarmContext(DbContextOptions<FarmContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rabbit>()
                .HasMany(r => r.Offspring)
                .WithOne(r => r.Mother)
                .HasForeignKey(r => r.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pig>()
                .HasMany(p => p.Offspring)
                .WithOne(p => p.Mother)
                .HasForeignKey(p => p.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cattle>()
                .HasMany(c => c.Offspring)
                .WithOne(c => c.Mother)
                .HasForeignKey(c => c.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Goat>()
                .HasMany(g => g.Offspring)
                .WithOne(g => g.Mother)
                .HasForeignKey(g => g.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Turkey>()
                .HasMany(t => t.Offspring)
                .WithOne(t => t.Mother)
                .HasForeignKey(t => t.MotherId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        // Add a new entry
        public async Task AddEntityAsync<T>(T entity) where T : class
        {
            try
            {
                Set<T>().Add(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding entity: {ex.Message}");
            }
        }

        // Update an existing entry
        public async Task UpdateEntityAsync<T>(T entity) where T : class
        {
            try
            {
                Set<T>().Update(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating entity: {ex.Message}");
            }
        }

        // Delete an entry
        public async Task DeleteEntityAsync<T>(T entity) where T : class
        {
            try
            {
                Set<T>().Remove(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity: {ex.Message}");
            }
        }
    }
}
