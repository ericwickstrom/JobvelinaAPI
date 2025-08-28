using System.Collections.Concurrent;
using Jobvelina.Application.Interfaces;
using Jobvelina.Core.Entities;
using Jobvelina.Core.Enums;

namespace Jobvelina.Application.Services;

/// <summary>
/// Mock implementation of IJobApplicationRepository for testing and development
/// Provides thread-safe in-memory storage with realistic sample data
/// </summary>
public class MockJobApplicationService : IJobApplicationRepository
{
    private readonly ConcurrentDictionary<Guid, JobApplication> _jobApplications;
    private readonly ConcurrentDictionary<Guid, Company> _companies;
    private readonly ConcurrentDictionary<Guid, JobPlatform> _jobPlatforms;
    private readonly object _lockObject = new();

    /// <summary>
    /// Initializes a new instance of MockJobApplicationService with sample data
    /// </summary>
    public MockJobApplicationService()
    {
        _jobApplications = new ConcurrentDictionary<Guid, JobApplication>();
        _companies = new ConcurrentDictionary<Guid, Company>();
        _jobPlatforms = new ConcurrentDictionary<Guid, JobPlatform>();
        SeedData();
    }

    /// <summary>
    /// Gets all non-deleted job applications
    /// </summary>
    /// <returns>A collection of job applications</returns>
    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        await Task.Delay(50); // Simulate async operation

        return _jobApplications.Values
            .Where(ja => !ja.IsDeleted)
            .OrderByDescending(ja => ja.CreateDate)
            .ToList();
    }

    /// <summary>
    /// Gets a job application by its ID
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>The job application if found, null otherwise</returns>
    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        await Task.Delay(25); // Simulate async operation

        if (id == Guid.Empty)
            return null;

        _jobApplications.TryGetValue(id, out var jobApplication);
        return jobApplication?.IsDeleted == false ? jobApplication : null;
    }

    /// <summary>
    /// Creates a new job application
    /// </summary>
    /// <param name="jobApplication">The job application to create</param>
    /// <returns>The created job application</returns>
    public async Task<JobApplication> CreateAsync(JobApplication jobApplication)
    {
        await Task.Delay(100); // Simulate async operation

        if (jobApplication == null)
            throw new ArgumentNullException(nameof(jobApplication));

        lock (_lockObject)
        {
            var now = DateTime.UtcNow;
            jobApplication.CreateDate = now;
            jobApplication.ModifiedDate = now;
            jobApplication.IsDeleted = false;

            _jobApplications.TryAdd(jobApplication.Id, jobApplication);
        }

        return jobApplication;
    }

    /// <summary>
    /// Updates an existing job application
    /// </summary>
    /// <param name="jobApplication">The job application to update</param>
    /// <returns>The updated job application</returns>
    public async Task<JobApplication> UpdateAsync(JobApplication jobApplication)
    {
        await Task.Delay(75); // Simulate async operation

        if (jobApplication == null)
            throw new ArgumentNullException(nameof(jobApplication));

        if (!_jobApplications.TryGetValue(jobApplication.Id, out var existingApplication) || existingApplication.IsDeleted)
            throw new InvalidOperationException($"Job application with ID {jobApplication.Id} not found or has been deleted.");

        lock (_lockObject)
        {
            // Update properties while preserving original create date and ID
            existingApplication.CompanyId = jobApplication.CompanyId;
            existingApplication.JobPlatformId = jobApplication.JobPlatformId;
            existingApplication.JobTitle = jobApplication.JobTitle;
            existingApplication.Status = jobApplication.Status;
            existingApplication.Notes = jobApplication.Notes;
            existingApplication.ModifiedDate = DateTime.UtcNow;
        }

        return existingApplication;
    }

    /// <summary>
    /// Soft deletes a job application by setting IsDeleted to true
    /// </summary>
    /// <param name="id">The ID of the job application to delete</param>
    /// <returns>True if the operation was successful, false otherwise</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        await Task.Delay(50); // Simulate async operation

        if (id == Guid.Empty)
            return false;

        if (!_jobApplications.TryGetValue(id, out var jobApplication) || jobApplication.IsDeleted)
            return false;

        lock (_lockObject)
        {
            jobApplication.IsDeleted = true;
            jobApplication.ModifiedDate = DateTime.UtcNow;
        }

        return true;
    }

    /// <summary>
    /// Checks if a job application exists and is not deleted
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>True if the job application exists and is not deleted, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        await Task.Delay(25); // Simulate async operation

        if (id == Guid.Empty)
            return false;

        return _jobApplications.TryGetValue(id, out var jobApplication) && !jobApplication.IsDeleted;
    }

    /// <summary>
    /// Seeds the mock service with realistic sample data
    /// </summary>
    private void SeedData()
    {
        // Seed companies
        var companyId1 = new Guid("11111111-1111-1111-1111-111111111111");
        var companyId2 = new Guid("22222222-2222-2222-2222-222222222222");
        var companyId3 = new Guid("33333333-3333-3333-3333-333333333333");
        
        var companies = new[]
        {
            new Company { Id = companyId1, Name = "Microsoft", Description = "Technology company", Industry = "Technology", CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
            new Company { Id = companyId2, Name = "Google", Description = "Search and technology company", Industry = "Technology", CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
            new Company { Id = companyId3, Name = "Amazon", Description = "E-commerce and cloud services", Industry = "Technology", CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
        };

        foreach (var company in companies)
        {
            _companies.TryAdd(company.Id, company);
        }

        // Seed job platforms
        var platformId1 = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var platformId2 = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var platformId3 = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");
        
        var platforms = new[]
        {
            new JobPlatform { Id = platformId1, Name = "LinkedIn", Description = "Professional networking platform", WebsiteUrl = "https://linkedin.com", IsActive = true, CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
            new JobPlatform { Id = platformId2, Name = "Indeed", Description = "Job search platform", WebsiteUrl = "https://indeed.com", IsActive = true, CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
            new JobPlatform { Id = platformId3, Name = "Company Website", Description = "Direct application through company website", IsActive = true, CreateDate = DateTime.UtcNow.AddDays(-100), ModifiedDate = DateTime.UtcNow.AddDays(-100) },
        };

        foreach (var platform in platforms)
        {
            _jobPlatforms.TryAdd(platform.Id, platform);
        }

        // Seed job applications
        var sampleData = new[]
        {
            new JobApplication
            {
                Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                CompanyId = companyId1,
                JobPlatformId = platformId1,
                Company = companies[0],
                JobPlatform = platforms[0],
                JobTitle = "Senior Software Engineer",
                CreateDate = DateTime.UtcNow.AddDays(-45),
                ModifiedDate = DateTime.UtcNow.AddDays(-10),
                Status = JobApplicationStatus.InterviewScheduled,
                Notes = "Initial screening completed. Technical interview scheduled for next week.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                CompanyId = companyId2,
                JobPlatformId = platformId3,
                Company = companies[1],
                JobPlatform = platforms[2],
                JobTitle = "Software Developer",
                CreateDate = DateTime.UtcNow.AddDays(-38),
                ModifiedDate = DateTime.UtcNow.AddDays(-38),
                Status = JobApplicationStatus.Applied,
                Notes = "Applied through careers page. Waiting for response.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                CompanyId = companyId3,
                JobPlatformId = platformId2,
                Company = companies[2],
                JobPlatform = platforms[1],
                JobTitle = "Full Stack Developer",
                CreateDate = DateTime.UtcNow.AddDays(-32),
                ModifiedDate = DateTime.UtcNow.AddDays(-5),
                Status = JobApplicationStatus.OfferReceived,
                Notes = "Completed all interview rounds. Offer received with competitive salary package.",
                IsDeleted = false
            }
        };

        foreach (var jobApplication in sampleData)
        {
            _jobApplications.TryAdd(jobApplication.Id, jobApplication);
        }
    }
}