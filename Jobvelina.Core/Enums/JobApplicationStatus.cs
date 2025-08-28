namespace Jobvelina.Core.Enums;

/// <summary>
/// Represents the status of a job application
/// </summary>
public enum JobApplicationStatus
{
    /// <summary>
    /// Application has been submitted
    /// </summary>
    Applied = 0,

    /// <summary>
    /// Application was rejected
    /// </summary>
    Rejected = 1,

    /// <summary>
    /// Interview has been scheduled
    /// </summary>
    InterviewScheduled = 2,

    /// <summary>
    /// Job offer has been received
    /// </summary>
    OfferReceived = 3,

    /// <summary>
    /// Application was withdrawn by the applicant
    /// </summary>
    Withdrawn = 4,

    /// <summary>
    /// Application is currently under review
    /// </summary>
    UnderReview = 5
}