using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Jobvelina.Persistence.Data;

/// <summary>
/// Design-time factory for creating ApplicationDbContext instances during migrations
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of ApplicationDbContext for design-time operations
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    /// <returns>A configured ApplicationDbContext instance</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use the same connection string as defined in appsettings.json
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=JobvelinaDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}