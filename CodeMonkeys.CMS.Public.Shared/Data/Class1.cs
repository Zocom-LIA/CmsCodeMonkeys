using CodeMonkeys.CMS.Public.Shared.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Configure your database provider here (example for SQL Server)
        optionsBuilder.UseSqlServer("DefaultConnection");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}