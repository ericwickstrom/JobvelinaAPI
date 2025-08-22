using System.ComponentModel.DataAnnotations;
using Jobvelina.Core.Enums;

namespace Jobvelina.Application.DTOs;

/// <summary>
/// DTO for creating a new job application
/// </summary>
public class CreateJobApplicationDto
{
    /// <summary>
    /// Name of the company
    /// </summary>
    [Required(ErrorMessage = "Company name is required")]
    [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// Title of the job position
    /// </summary>
    [Required(ErrorMessage = "Job title is required")]
    [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters")]
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Platform where the application was submitted
    /// </summary>
    [Required(ErrorMessage = "Platform is required")]
    [StringLength(50, ErrorMessage = "Platform cannot exceed 50 characters")]
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the job application
    /// </summary>
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Applied;

    /// <summary>
    /// Optional notes about the application
    /// </summary>
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }
}