namespace VagaFlow.Api.Jobs;

public enum JobArea
{
    It,
    Logistics,
    Administration,
    CustomerService,
    Other
}

public enum WorkArrangement
{
    Unknown,
    Remote,
    Hybrid,
    OnSite
}

public enum JobVerificationState
{
    PendingReview,
    Verified,
    Unavailable
}

/// <summary>
/// Published date is independently evidenced; FirstSeenAtUtc is *only* when the
/// VagaFlow observed/imported the listing. Never infer PublishedAtUtc from FirstSeenAtUtc.
/// </summary>
public sealed class JobListing
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string CanonicalUrl { get; set; } = string.Empty;
    public string SourcePortal { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public JobArea Area { get; set; } = JobArea.Other;
    public WorkArrangement WorkArrangement { get; set; } = WorkArrangement.Unknown;
    public string? City { get; set; }
    public string? State { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
    public DateTime FirstSeenAtUtc { get; set; }
    public JobVerificationState VerificationState { get; set; } = JobVerificationState.PendingReview;
}
