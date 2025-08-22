using Jobvelina.Core.Entities;

namespace Jobvelina.Application.Interfaces;

/// <summary>
/// Repository interface for JobApplication operations
/// </summary>
public interface IJobApplicationRepository
{
    /// <summary>
    /// Gets all non-deleted job applications
    /// </summary>
    /// <returns>A collection of job applications</returns>
    Task<IEnumerable<JobApplication>> GetAllAsync();

    /// <summary>
    /// Gets a job application by its ID
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>The job application if found, null otherwise</returns>
    Task<JobApplication?> GetByIdAsync(string id);

    /// <summary>
    /// Creates a new job application
    /// </summary>
    /// <param name="jobApplication">The job application to create</param>
    /// <returns>The created job application</returns>
    Task<JobApplication> CreateAsync(JobApplication jobApplication);

    /// <summary>
    /// Updates an existing job application
    /// </summary>
    /// <param name="jobApplication">The job application to update</param>
    /// <returns>The updated job application</returns>
    Task<JobApplication> UpdateAsync(JobApplication jobApplication);

    /// <summary>
    /// Soft deletes a job application by setting IsDeleted to true
    /// </summary>
    /// <param name="id">The ID of the job application to delete</param>
    /// <returns>True if the operation was successful, false otherwise</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Checks if a job application exists and is not deleted
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>True if the job application exists and is not deleted, false otherwise</returns>
    Task<bool> ExistsAsync(string id);
}