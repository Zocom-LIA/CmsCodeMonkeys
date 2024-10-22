using CodeMonkeys.CMS.Public.Shared.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Justera BasePath till att peka på katalogen där appsettings.json finns
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "CodeMonkeys.CMS.Public"))
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found.");
        }

        optionsBuilder.UseSqlServer(connectionString);

        var logger = LoggerFactory.Create(builder => builder.AddProvider(new ConsoleLoggerProvider(LogLevel.Debug)))
            .CreateLogger<ApplicationDbContext>();

        return new ApplicationDbContext(optionsBuilder.Options, logger);
    }
}