using VagaFlow.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint intentionally independent of database/external services.
// Future readiness checks must not disclose personal data or secrets.
app.MapGet("/health", () => Results.Ok(new HealthStatus("healthy", "VagaFlow.Api", "0.1.0-alpha")))
    .WithName("Health")
    .WithTags("Operations");

app.Run();

// Public for WebApplicationFactory integration tests.
public partial class Program;
