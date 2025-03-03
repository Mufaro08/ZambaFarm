using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ZambaFarm.Models
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FarmContext>
    {
        public FarmContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FarmContext>();
            var connectionString = configuration.GetConnectionString("FarmContext");

            builder.UseSqlServer(connectionString);

            return new FarmContext(builder.Options);
        }
    }
}
