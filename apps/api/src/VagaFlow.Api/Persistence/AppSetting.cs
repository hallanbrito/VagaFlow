namespace VagaFlow.Api.Persistence;

/// <summary>
/// Technical local application setting (never store passwords or clinical data here).
/// Future business entities will have their own migrations and constraints.
/// </summary>
public sealed class AppSetting
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime UpdatedAtUtc { get; set; }
}
