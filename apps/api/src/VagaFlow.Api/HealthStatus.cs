namespace VagaFlow.Api;

/// <summary>
/// Public, stable health check payload. Never include environment secrets.
/// </summary>
public sealed record HealthStatus(string Status, string Service, string Version);
