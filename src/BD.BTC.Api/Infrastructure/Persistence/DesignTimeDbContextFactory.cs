using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Try multiple methods to get the connection string
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            
            // If environment variable is not set, try reading from appsettings.json
            if (string.IsNullOrEmpty(connectionString))
            {
                try 
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddJsonFile("appsettings.Development.json", optional: true)
                        .Build();

                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                catch
                {
                    // If we can't read from config files, use a hardcoded fallback
                }
            }
            
            // Fallback connection string if all else fails
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Database=RedDrop;Username=postgres;Password=postgres";
            }
            
            // Configure the DbContext options and return the context
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}