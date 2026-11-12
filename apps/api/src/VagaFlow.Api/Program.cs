using Microsoft.EntityFrameworkCore;
using VagaFlow.Api;
using VagaFlow.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Database is optional for /health and basic tests. When configured, routes may
// resolve VagaFlowDbContext. Migrations are explicit (never run at startup).
var connectionString = builder.Configuration.GetConnectionString("VagaFlow");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<VagaFlowDbContext>(options => options.UseNpgsql(connectionString));
}

var app = builder.Build();

// Endpoint intentionally independent of database/external services.
// Future readiness checks must not disclose personal data or secrets.
app.MapGet("/health", () => Results.Ok(new HealthStatus("healthy", "VagaFlow.Api", "0.1.0-alpha")))
    .WithName("Health")
    .WithTags("Operations");

app.Run();

// Public for WebApplicationFactory integration tests.
public partial class Program { }
