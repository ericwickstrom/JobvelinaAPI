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
    private readonly ConcurrentDictionary<string, JobApplication> _jobApplications;
    private readonly object _lockObject = new();

    /// <summary>
    /// Initializes a new instance of MockJobApplicationService with sample data
    /// </summary>
    public MockJobApplicationService()
    {
        _jobApplications = new ConcurrentDictionary<string, JobApplication>();
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
    public async Task<JobApplication?> GetByIdAsync(string id)
    {
        await Task.Delay(25); // Simulate async operation

        if (string.IsNullOrWhiteSpace(id))
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
            jobApplication.Id = Guid.NewGuid().ToString();
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
            existingApplication.Company = jobApplication.Company;
            existingApplication.JobTitle = jobApplication.JobTitle;
            existingApplication.Platform = jobApplication.Platform;
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
    public async Task<bool> DeleteAsync(string id)
    {
        await Task.Delay(50); // Simulate async operation

        if (string.IsNullOrWhiteSpace(id))
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
    public async Task<bool> ExistsAsync(string id)
    {
        await Task.Delay(25); // Simulate async operation

        if (string.IsNullOrWhiteSpace(id))
            return false;

        return _jobApplications.TryGetValue(id, out var jobApplication) && !jobApplication.IsDeleted;
    }

    /// <summary>
    /// Seeds the mock service with realistic sample data
    /// </summary>
    private void SeedData()
    {
        var sampleData = new List<JobApplication>
        {
            new JobApplication
            {
                Id = "ja-001",
                Company = "Microsoft",
                JobTitle = "Senior Software Engineer",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddDays(-45),
                ModifiedDate = DateTime.UtcNow.AddDays(-10),
                Status = JobApplicationStatus.InterviewScheduled,
                Notes = "Initial screening completed. Technical interview scheduled for next week.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-002",
                Company = "Google",
                JobTitle = "Software Developer",
                Platform = "Company Website",
                CreateDate = DateTime.UtcNow.AddDays(-38),
                ModifiedDate = DateTime.UtcNow.AddDays(-38),
                Status = JobApplicationStatus.Applied,
                Notes = "Applied through careers page. Waiting for response.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-003",
                Company = "Amazon",
                JobTitle = "Full Stack Developer",
                Platform = "Indeed",
                CreateDate = DateTime.UtcNow.AddDays(-32),
                ModifiedDate = DateTime.UtcNow.AddDays(-5),
                Status = JobApplicationStatus.OfferReceived,
                Notes = "Completed all interview rounds. Offer received with competitive salary package.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-004",
                Company = "Meta",
                JobTitle = "Frontend Engineer",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddDays(-28),
                ModifiedDate = DateTime.UtcNow.AddDays(-15),
                Status = JobApplicationStatus.Rejected,
                Notes = "Completed technical assessment but didn't proceed to final round.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-005",
                Company = "Apple",
                JobTitle = "iOS Developer",
                Platform = "AngelList",
                CreateDate = DateTime.UtcNow.AddDays(-25),
                ModifiedDate = DateTime.UtcNow.AddDays(-25),
                Status = JobApplicationStatus.Applied,
                Notes = null,
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-006",
                Company = "Netflix",
                JobTitle = "Backend Engineer",
                Platform = "Glassdoor",
                CreateDate = DateTime.UtcNow.AddDays(-22),
                ModifiedDate = DateTime.UtcNow.AddDays(-3),
                Status = JobApplicationStatus.UnderReview,
                Notes = "Application is under review by the hiring team.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-007",
                Company = "Spotify",
                JobTitle = "DevOps Engineer",
                Platform = "Stack Overflow Jobs",
                CreateDate = DateTime.UtcNow.AddDays(-20),
                ModifiedDate = DateTime.UtcNow.AddDays(-8),
                Status = JobApplicationStatus.Withdrawn,
                Notes = "Withdrew application due to relocation requirements.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-008",
                Company = "Salesforce",
                JobTitle = "Cloud Architect",
                Platform = "Company Website",
                CreateDate = DateTime.UtcNow.AddDays(-18),
                ModifiedDate = DateTime.UtcNow.AddDays(-2),
                Status = JobApplicationStatus.InterviewScheduled,
                Notes = "Phone screening passed. On-site interview scheduled.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-009",
                Company = "Uber",
                JobTitle = "Mobile Developer",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddDays(-15),
                ModifiedDate = DateTime.UtcNow.AddDays(-15),
                Status = JobApplicationStatus.Applied,
                Notes = "Strong interest in their ride-sharing technology stack.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-010",
                Company = "Airbnb",
                JobTitle = "Product Manager",
                Platform = "Indeed",
                CreateDate = DateTime.UtcNow.AddDays(-12),
                ModifiedDate = DateTime.UtcNow.AddDays(-7),
                Status = JobApplicationStatus.Rejected,
                Notes = "Feedback: Great technical skills but looking for more PM experience.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-011",
                Company = "Stripe",
                JobTitle = "Payment Systems Engineer",
                Platform = "AngelList",
                CreateDate = DateTime.UtcNow.AddDays(-10),
                ModifiedDate = DateTime.UtcNow.AddDays(-1),
                Status = JobApplicationStatus.UnderReview,
                Notes = "Completed coding challenge. Waiting for feedback from engineering team.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-012",
                Company = "Slack",
                JobTitle = "Frontend Developer",
                Platform = "Company Website",
                CreateDate = DateTime.UtcNow.AddDays(-8),
                ModifiedDate = DateTime.UtcNow.AddDays(-8),
                Status = JobApplicationStatus.Applied,
                Notes = null,
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-013",
                Company = "Discord",
                JobTitle = "Software Engineer",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddDays(-6),
                ModifiedDate = DateTime.UtcNow.AddDays(-4),
                Status = JobApplicationStatus.InterviewScheduled,
                Notes = "Initial call went well. Technical round scheduled for this week.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-014",
                Company = "Zoom",
                JobTitle = "Video Technology Engineer",
                Platform = "Glassdoor",
                CreateDate = DateTime.UtcNow.AddDays(-5),
                ModifiedDate = DateTime.UtcNow.AddDays(-5),
                Status = JobApplicationStatus.Applied,
                Notes = "Interested in their video conferencing infrastructure.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-015",
                Company = "Twitch",
                JobTitle = "Backend Developer",
                Platform = "Stack Overflow Jobs",
                CreateDate = DateTime.UtcNow.AddDays(-3),
                ModifiedDate = DateTime.UtcNow.AddDays(-3),
                Status = JobApplicationStatus.Applied,
                Notes = "Gaming industry experience would be valuable here.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-016",
                Company = "Shopify",
                JobTitle = "E-commerce Developer",
                Platform = "Indeed",
                CreateDate = DateTime.UtcNow.AddDays(-2),
                ModifiedDate = DateTime.UtcNow.AddDays(-2),
                Status = JobApplicationStatus.Applied,
                Notes = null,
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-017",
                Company = "DocuSign",
                JobTitle = "Security Engineer",
                Platform = "Company Website",
                CreateDate = DateTime.UtcNow.AddDays(-1),
                ModifiedDate = DateTime.UtcNow.AddDays(-1),
                Status = JobApplicationStatus.Applied,
                Notes = "Focus on document security and encryption technologies.",
                IsDeleted = false
            },
            new JobApplication
            {
                Id = "ja-018",
                Company = "Atlassian",
                JobTitle = "DevOps Specialist",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddHours(-12),
                ModifiedDate = DateTime.UtcNow.AddHours(-12),
                Status = JobApplicationStatus.Applied,
                Notes = "Experience with Jira and Confluence would be beneficial.",
                IsDeleted = false
            },
            // Adding one soft-deleted entry to test filtering
            new JobApplication
            {
                Id = "ja-deleted-001",
                Company = "Old Company",
                JobTitle = "Deleted Position",
                Platform = "LinkedIn",
                CreateDate = DateTime.UtcNow.AddDays(-60),
                ModifiedDate = DateTime.UtcNow.AddDays(-30),
                Status = JobApplicationStatus.Withdrawn,
                Notes = "This application was withdrawn and soft deleted.",
                IsDeleted = true
            }
        };

        foreach (var jobApplication in sampleData)
        {
            _jobApplications.TryAdd(jobApplication.Id, jobApplication);
        }
    }
}