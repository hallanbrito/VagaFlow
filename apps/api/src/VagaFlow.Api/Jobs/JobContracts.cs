namespace VagaFlow.Api.Jobs;

public sealed record ManualJobRequest(
    string? Company,
    string? Title,
    string? Url,
    string? SourcePortal,
    string? Description,
    JobArea Area = JobArea.Other,
    WorkArrangement WorkArrangement = WorkArrangement.Unknown,
    string? City = null,
    string? State = null,
    DateTimeOffset? PublishedAtUtc = null,
    bool PublicationDateVerified = false
);

public sealed record JobResponse(
    Guid Id,
    string Company,
    string Title,
    string OriginalUrl,
    string SourcePortal,
    string Description,
    JobArea Area,
    WorkArrangement WorkArrangement,
    string? City,
    string? State,
    DateTime? PublishedAtUtc,
    DateTime FirstSeenAtUtc,
    JobVerificationState VerificationState)
{
    public static JobResponse FromEntity(JobListing job) => new(
        job.Id,
        job.Company,
        job.Title,
        job.OriginalUrl,
        job.SourcePortal,
        job.Description,
        job.Area,
        job.WorkArrangement,
        job.City,
        job.State,
        job.PublishedAtUtc,
        job.FirstSeenAtUtc,
        job.VerificationState);
}
