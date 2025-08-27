namespace Jobvelina.Core.Entities;

/// <summary>
/// Represents a company entity
/// </summary>
public class Company
{
    /// <summary>
    /// Unique identifier for the company
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the company
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Company description or notes
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Company website URL
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Industry the company operates in
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Date when the company record was created
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Date when the company record was last modified
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Navigation property for job applications
    /// </summary>
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}