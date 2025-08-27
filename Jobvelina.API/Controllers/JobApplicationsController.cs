using Jobvelina.Application.DTOs;
using Jobvelina.Application.Interfaces;
using Jobvelina.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Jobvelina.API.Controllers;

/// <summary>
/// Controller for managing job applications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly ILogger<JobApplicationsController> _logger;

    /// <summary>
    /// Initializes a new instance of the JobApplicationsController
    /// </summary>
    /// <param name="jobApplicationRepository">The job application repository</param>
    /// <param name="logger">The logger</param>
    public JobApplicationsController(
        IJobApplicationRepository jobApplicationRepository,
        ILogger<JobApplicationsController> logger)
    {
        _jobApplicationRepository = jobApplicationRepository ?? throw new ArgumentNullException(nameof(jobApplicationRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all non-deleted job applications
    /// </summary>
    /// <returns>A list of job applications</returns>
    /// <response code="200">Returns the list of job applications</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllJobApplications()
    {
        try
        {
            _logger.LogInformation("Getting all job applications");
            
            var jobApplications = await _jobApplicationRepository.GetAllAsync();
            var jobApplicationDtos = jobApplications.Select(MapToDto);
            
            _logger.LogInformation("Retrieved {Count} job applications", jobApplicationDtos.Count());
            return Ok(jobApplicationDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving job applications");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving job applications");
        }
    }

    /// <summary>
    /// Gets a specific job application by ID
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>The job application if found</returns>
    /// <response code="200">Returns the job application</response>
    /// <response code="404">If the job application is not found</response>
    /// <response code="400">If the ID is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JobApplicationDto>> GetJobApplication(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("GetJobApplication called with null or empty ID");
                return BadRequest("Job application ID cannot be null or empty");
            }

            _logger.LogInformation("Getting job application with ID: {Id}", id);
            
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            
            if (jobApplication == null)
            {
                _logger.LogWarning("Job application with ID {Id} not found", id);
                return NotFound($"Job application with ID {id} not found");
            }

            var jobApplicationDto = MapToDto(jobApplication);
            _logger.LogInformation("Retrieved job application with ID: {Id}", id);
            
            return Ok(jobApplicationDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving job application with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the job application");
        }
    }

    /// <summary>
    /// Creates a new job application
    /// </summary>
    /// <param name="createJobApplicationDto">The job application data</param>
    /// <returns>The created job application</returns>
    /// <response code="201">Returns the newly created job application</response>
    /// <response code="400">If the job application data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JobApplicationDto>> CreateJobApplication(CreateJobApplicationDto createJobApplicationDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating job application");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new job application for company: {Company}", createJobApplicationDto.Company);
            
            var jobApplication = MapFromCreateDto(createJobApplicationDto);
            var createdJobApplication = await _jobApplicationRepository.CreateAsync(jobApplication);
            var jobApplicationDto = MapToDto(createdJobApplication);
            
            _logger.LogInformation("Created job application with ID: {Id}", createdJobApplication.Id);
            
            return CreatedAtAction(
                nameof(GetJobApplication),
                new { id = createdJobApplication.Id },
                jobApplicationDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating job application");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the job application");
        }
    }

    /// <summary>
    /// Updates an existing job application
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <param name="updateJobApplicationDto">The updated job application data</param>
    /// <returns>The updated job application</returns>
    /// <response code="200">Returns the updated job application</response>
    /// <response code="400">If the job application data is invalid</response>
    /// <response code="404">If the job application is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JobApplicationDto>> UpdateJobApplication(string id, UpdateJobApplicationDto updateJobApplicationDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("UpdateJobApplication called with null or empty ID");
                return BadRequest("Job application ID cannot be null or empty");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating job application with ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating job application with ID: {Id}", id);
            
            var existingJobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (existingJobApplication == null)
            {
                _logger.LogWarning("Job application with ID {Id} not found for update", id);
                return NotFound($"Job application with ID {id} not found");
            }

            var jobApplicationToUpdate = MapFromUpdateDto(updateJobApplicationDto, id);
            var updatedJobApplication = await _jobApplicationRepository.UpdateAsync(jobApplicationToUpdate);
            var jobApplicationDto = MapToDto(updatedJobApplication);
            
            _logger.LogInformation("Updated job application with ID: {Id}", id);
            
            return Ok(jobApplicationDto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Job application with ID {Id} not found for update", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating job application with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the job application");
        }
    }

    /// <summary>
    /// Soft deletes a job application
    /// </summary>
    /// <param name="id">The job application ID</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the job application was successfully deleted</response>
    /// <response code="400">If the ID is invalid</response>
    /// <response code="404">If the job application is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteJobApplication(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("DeleteJobApplication called with null or empty ID");
                return BadRequest("Job application ID cannot be null or empty");
            }

            _logger.LogInformation("Deleting job application with ID: {Id}", id);
            
            var exists = await _jobApplicationRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Job application with ID {Id} not found for deletion", id);
                return NotFound($"Job application with ID {id} not found");
            }

            var deleted = await _jobApplicationRepository.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete job application with ID: {Id}", id);
                return NotFound($"Job application with ID {id} not found");
            }
            
            _logger.LogInformation("Deleted job application with ID: {Id}", id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting job application with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the job application");
        }
    }

    /// <summary>
    /// Maps a JobApplication entity to a JobApplicationDto
    /// </summary>
    /// <param name="jobApplication">The job application entity</param>
    /// <returns>The job application DTO</returns>
    private static JobApplicationDto MapToDto(JobApplication jobApplication)
    {
        return new JobApplicationDto
        {
            Id = jobApplication.Id,
            Company = jobApplication.Company?.Name ?? string.Empty,
            JobTitle = jobApplication.JobTitle,
            Platform = jobApplication.JobPlatform?.Name ?? string.Empty,
            CreateDate = jobApplication.CreateDate,
            ModifiedDate = jobApplication.ModifiedDate,
            Status = jobApplication.Status,
            Notes = jobApplication.Notes
        };
    }

    /// <summary>
    /// Maps a CreateJobApplicationDto to a JobApplication entity
    /// </summary>
    /// <param name="createDto">The create DTO</param>
    /// <returns>The job application entity</returns>
    private static JobApplication MapFromCreateDto(CreateJobApplicationDto createDto)
    {
        // For simplicity in this Entity Framework setup, we'll use placeholder IDs
        // In a real application, you would look up existing companies and platforms
        // or create new ones as needed
        return new JobApplication
        {
            CompanyId = "comp-001", // Placeholder - would be looked up or created
            JobPlatformId = "plat-001", // Placeholder - would be looked up or created
            JobTitle = createDto.JobTitle,
            Status = createDto.Status,
            Notes = createDto.Notes
        };
    }

    /// <summary>
    /// Maps an UpdateJobApplicationDto to a JobApplication entity
    /// </summary>
    /// <param name="updateDto">The update DTO</param>
    /// <param name="id">The job application ID</param>
    /// <returns>The job application entity</returns>
    private static JobApplication MapFromUpdateDto(UpdateJobApplicationDto updateDto, string id)
    {
        // For simplicity in this Entity Framework setup, we'll use placeholder IDs
        // In a real application, you would look up existing companies and platforms
        // or create new ones as needed
        return new JobApplication
        {
            Id = id,
            CompanyId = "comp-001", // Placeholder - would be looked up or created
            JobPlatformId = "plat-001", // Placeholder - would be looked up or created
            JobTitle = updateDto.JobTitle,
            Status = updateDto.Status,
            Notes = updateDto.Notes
        };
    }
}