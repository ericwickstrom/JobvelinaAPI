using Jobvelina.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobvelina.Persistence.Data;

/// <summary>
/// Entity Framework database context for the application
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApplicationDbContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for JobApplication entities
    /// </summary>
    public DbSet<JobApplication> JobApplications { get; set; }

    /// <summary>
    /// DbSet for Company entities
    /// </summary>
    public DbSet<Company> Companies { get; set; }

    /// <summary>
    /// DbSet for JobPlatform entities
    /// </summary>
    public DbSet<JobPlatform> JobPlatforms { get; set; }

    /// <summary>
    /// Configures the model and relationships
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Company entity
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(200);

            entity.Property(e => e.Industry)
                .HasMaxLength(100);

            entity.Property(e => e.CreateDate)
                .IsRequired();

            entity.Property(e => e.ModifiedDate)
                .IsRequired();

            // Create index for common queries
            entity.HasIndex(e => e.Name);
        });

        // Configure JobPlatform entity
        modelBuilder.Entity<JobPlatform>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(200);

            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.Property(e => e.CreateDate)
                .IsRequired();

            entity.Property(e => e.ModifiedDate)
                .IsRequired();

            // Create index for common queries
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
        });

        // Configure JobApplication entity
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CompanyId)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.JobPlatformId)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CreateDate)
                .IsRequired();

            entity.Property(e => e.ModifiedDate)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.IsDeleted)
                .IsRequired();

            // Configure relationships
            entity.HasOne(e => e.Company)
                .WithMany(c => c.JobApplications)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.JobPlatform)
                .WithMany(p => p.JobApplications)
                .HasForeignKey(e => e.JobPlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            // Create index for common queries
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreateDate);
            entity.HasIndex(e => e.CompanyId);
            entity.HasIndex(e => e.JobPlatformId);
        });
    }
}