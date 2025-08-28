using Jobvelina.Core.Enums;

namespace Jobvelina.Core.Entities;

/// <summary>
/// Represents a job application entity
/// </summary>
public class JobApplication
{
    /// <summary>
    /// Unique identifier for the job application
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key for the company
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Navigation property for the company
    /// </summary>
    public Company Company { get; set; } = null!;

    /// <summary>
    /// Foreign key for the job platform
    /// </summary>
    public Guid JobPlatformId { get; set; }

    /// <summary>
    /// Navigation property for the job platform
    /// </summary>
    public JobPlatform JobPlatform { get; set; } = null!;

    /// <summary>
    /// Title of the job position
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Date when the application was created
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Date when the application was last modified
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Current status of the job application
    /// </summary>
    public JobApplicationStatus Status { get; set; }

    /// <summary>
    /// Optional notes about the application
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates whether the application has been soft deleted
    /// </summary>
    public bool IsDeleted { get; set; }
}