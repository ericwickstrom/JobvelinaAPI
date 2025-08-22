using Jobvelina.Application.Interfaces;
using Jobvelina.Core.Entities;
using Jobvelina.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobvelina.Persistence.Repositories;

/// <summary>
/// Repository implementation for JobApplication operations
/// </summary>
public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the JobApplicationRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public JobApplicationRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets all non-deleted job applications
    /// </summary>
    /// <returns>A collection of job applications</returns>
    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await _context.JobApplications
            .Where(ja => !ja.IsDeleted)
            .OrderByDescending(ja => ja.CreateDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a job application by its ID
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>The job application if found, null otherwise</returns>
    public async Task<JobApplication?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.Id == id && !ja.IsDeleted);
    }

    /// <summary>
    /// Creates a new job application
    /// </summary>
    /// <param name="jobApplication">The job application to create</param>
    /// <returns>The created job application</returns>
    public async Task<JobApplication> CreateAsync(JobApplication jobApplication)
    {
        if (jobApplication == null)
            throw new ArgumentNullException(nameof(jobApplication));

        var now = DateTime.UtcNow;
        jobApplication.Id = Guid.NewGuid().ToString();
        jobApplication.CreateDate = now;
        jobApplication.ModifiedDate = now;
        jobApplication.IsDeleted = false;

        _context.JobApplications.Add(jobApplication);
        await _context.SaveChangesAsync();

        return jobApplication;
    }

    /// <summary>
    /// Updates an existing job application
    /// </summary>
    /// <param name="jobApplication">The job application to update</param>
    /// <returns>The updated job application</returns>
    public async Task<JobApplication> UpdateAsync(JobApplication jobApplication)
    {
        if (jobApplication == null)
            throw new ArgumentNullException(nameof(jobApplication));

        var existingApplication = await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.Id == jobApplication.Id && !ja.IsDeleted);

        if (existingApplication == null)
            throw new InvalidOperationException($"Job application with ID {jobApplication.Id} not found or has been deleted.");

        // Update properties
        existingApplication.Company = jobApplication.Company;
        existingApplication.JobTitle = jobApplication.JobTitle;
        existingApplication.Platform = jobApplication.Platform;
        existingApplication.Status = jobApplication.Status;
        existingApplication.Notes = jobApplication.Notes;
        existingApplication.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingApplication;
    }

    /// <summary>
    /// Soft deletes a job application by setting IsDeleted to true
    /// </summary>
    /// <param name="id">The ID of the job application to delete</param>
    /// <returns>True if the operation was successful, false otherwise</returns>
    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return false;

        var jobApplication = await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.Id == id && !ja.IsDeleted);

        if (jobApplication == null)
            return false;

        jobApplication.IsDeleted = true;
        jobApplication.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Checks if a job application exists and is not deleted
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>True if the job application exists and is not deleted, false otherwise</returns>
    public async Task<bool> ExistsAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return false;

        return await _context.JobApplications
            .AnyAsync(ja => ja.Id == id && !ja.IsDeleted);
    }
}