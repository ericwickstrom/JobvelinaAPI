using Jobvelina.Core.Enums;

namespace Jobvelina.Application.DTOs;

/// <summary>
/// DTO for returning job application data
/// </summary>
public class JobApplicationDto
{
    /// <summary>
    /// Unique identifier for the job application
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the company
    /// </summary>
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// Title of the job position
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Platform where the application was submitted
    /// </summary>
    public string Platform { get; set; } = string.Empty;

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
}