using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ZambaFarm.Models;

namespace ZambaFarm.Models
{
    public class FarmContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Rabbit> Rabbits { get; set; }
        public DbSet<Pig> Pigs { get; set; }
        public DbSet<Cattle> Cattles { get; set; }
        public DbSet<Goat> Goats { get; set; }
        public DbSet<Turkey> Turkeys { get; set; }
       // public DbSet<ApplicationUser> Users { get; set; } = default!;

        public FarmContext(DbContextOptions<FarmContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customize table name for Turkey
            modelBuilder.Entity<Turkey>().ToTable("turkeys");

            // Configure MotherTagNumber for different entities
            modelBuilder.Entity<Rabbit>().Property(r => r.MotherTagNumber).HasColumnName("MotherTagNumber");
            modelBuilder.Entity<Pig>().Property(p => p.MotherTagNumber).HasColumnName("MotherTagNumber");
            modelBuilder.Entity<Cattle>().Property(c => c.MotherTagNumber).HasColumnName("MotherTagNumber");
            modelBuilder.Entity<Goat>().Property(g => g.MotherTagNumber).HasColumnName("MotherTagNumber");
            modelBuilder.Entity<Turkey>().Property(t => t.MotherTagNumber).HasColumnName("MotherTagNumber");

            // Configure Identity roles and users
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
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










/*using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ZambaFarm.Models;

namespace ZambaFarm.Models
{
    public class FarmContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Rabbit> Rabbits { get; set; }
        public DbSet<Pig> Pigs { get; set; }
        public DbSet<Cattle> Cattles { get; set; }
        public DbSet<Goat> Goats { get; set; }
        public DbSet<Turkey> Turkeys { get; set; }
        public DbSet<users> users { get; set; } = default!;

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

            // Configure MotherTagNumber for different entities
            modelBuilder.Entity<Rabbit>()
                .Property(r => r.MotherTagNumber)
                .HasColumnName("MotherTagNumber");

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

            // Configure Identity roles and users if needed
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
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
*/








