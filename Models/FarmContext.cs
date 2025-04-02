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

            // Customize table name for Turkey
            modelBuilder.Entity<Turkey>()
                .ToTable("turkeys");

            // Configure MotherTagNumber for Rabbit without foreign key relation
            modelBuilder.Entity<Rabbit>()
                .Property(r => r.MotherTagNumber)
                .HasColumnName("MotherTagNumber");

            // Prevent cascading deletes for other entities
            modelBuilder.Entity<Pig>()
                .Property(p => p.MotherTagNumber)
                .HasColumnName("MotherTagNumber");

            modelBuilder.Entity<Cattle>()
                .Property(c => c.MotherTagNumber)
                .HasColumnName("MotherTagNumber");

            modelBuilder.Entity<Goat>()
                .Property(g => g.MotherTagNumber)
                .HasColumnName("MotherTagNumber");

            modelBuilder.Entity<Turkey>()
                .Property(t => t.MotherTagNumber)
                .HasColumnName("MotherTagNumber");
        }

        // Add a new entity
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

        // Update an entity
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

        // Delete an entity
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
