namespace Jobvelina.Core.Entities;

/// <summary>
/// Represents a job platform entity (e.g., LinkedIn, Indeed, etc.)
/// </summary>
public class JobPlatform
{
    /// <summary>
    /// Unique identifier for the job platform
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the job platform
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the job platform
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Website URL of the job platform
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Indicates whether the platform is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the platform record was created
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Date when the platform record was last modified
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Navigation property for job applications
    /// </summary>
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}