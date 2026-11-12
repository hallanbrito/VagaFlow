using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using VagaFlow.Api;
using VagaFlow.Api.Persistence;
using VagaFlow.Api.Jobs;

var builder = WebApplication.CreateBuilder(args);
// Database is optional for /health and basic tests. When configured, routes may
// resolve VagaFlowDbContext. Migrations are explicit (never run at startup).
var connectionString = builder.Configuration.GetConnectionString("VagaFlow");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<VagaFlowDbContext>(options => options.UseNpgsql(connectionString));
}

// Stable camelCase enum values for the Angular API client. Numeric enum values
// are rejected so future enum reordering cannot silently misclassify a vacancy.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)));

builder.Services.AddCors(options => options.AddPolicy("LocalAngular", policy =>
    policy.WithOrigins("http://127.0.0.1:4200", "http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalAngular");
}

app.MapJobEndpoints();

// Endpoint intentionally independent of database/external services.
// Future readiness checks must not disclose personal data or secrets.
app.MapGet("/health", () => Results.Ok(new HealthStatus("healthy", "VagaFlow.Api", "0.1.0-alpha")))
    .WithName("Health")
    .WithTags("Operations");

app.Run();

// Public for WebApplicationFactory integration tests.
public partial class Program { }
