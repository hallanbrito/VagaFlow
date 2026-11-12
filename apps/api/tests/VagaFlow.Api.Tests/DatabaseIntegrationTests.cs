using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using VagaFlow.Api.Persistence;
using VagaFlow.Api.Jobs;
using Xunit;

namespace VagaFlow.Api.Tests;

/// <summary>
/// Executes real PostgreSQL migrations and round-trip persistence in CI.
/// Local dotnet test runs without VAGAFLOW_TEST_DB_CONNECTION skip the database scenario.
/// </summary>
public sealed class DatabaseIntegrationTests
{
    [Fact]
    public async Task Postgres_MigrationsAndRoundTrip_Work()
    {
        var connectionString = Environment.GetEnvironmentVariable("VAGAFLOW_TEST_DB_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            // CI always provides this variable and a running PostgreSQL service.
            // Keep local API unit tests runnable without Docker.
            return;
        }

        var options = new DbContextOptionsBuilder<VagaFlowDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        var uniqueKey = $"integration_{Guid.NewGuid():N}";
        var jobId = Guid.NewGuid();
        var canonicalUrl = $"https://jobs.example.org/fixture/{jobId:N}";
        await using (var db = new VagaFlowDbContext(options))
        {
            await db.Database.MigrateAsync();
            var applied = await db.Database.GetAppliedMigrationsAsync();

            Assert.Contains("20261112190000_InitialInfrastructure", applied);
            Assert.Contains("20261112203000_AddJobListings", applied);

            db.AppSettings.Add(new AppSetting
            {
                Key = uniqueKey,
                Value = "test-ok",
                UpdatedAtUtc = DateTime.UtcNow
            });

            Assert.Equal(1, await db.SaveChangesAsync());

            db.Jobs.Add(new JobListing
            {
                Id = jobId,
                Company = "Empresa Ficticia CI",
                Title = "Analista de Testes",
                OriginalUrl = canonicalUrl,
                CanonicalUrl = canonicalUrl,
                SourcePortal = "ci-fixture",
                Description = "Vaga sintética de integração. Não é oportunidade real.",
                Area = JobArea.It,
                WorkArrangement = WorkArrangement.Remote,
                FirstSeenAtUtc = DateTime.UtcNow,
                PublishedAtUtc = null,
                VerificationState = JobVerificationState.PendingReview
            });
            Assert.Equal(1, await db.SaveChangesAsync());
        }

        await using (var db = new VagaFlowDbContext(options))
        {
            var saved = await db.AppSettings.AsNoTracking()
                .SingleAsync(setting => setting.Key == uniqueKey);

            Assert.Equal("test-ok", saved.Value);

            var job = await db.Jobs.AsNoTracking().SingleAsync(x => x.Id == jobId);
            Assert.Equal("Empresa Ficticia CI", job.Company);
            Assert.Equal(JobArea.It, job.Area);
            Assert.Null(job.PublishedAtUtc);
            Assert.Equal(JobVerificationState.PendingReview, job.VerificationState);

            db.AppSettings.Remove(saved);
            db.Jobs.Remove(job);
            Assert.Equal(2, await db.SaveChangesAsync());
        }
    }
    [Fact]
    public async Task ManualJobsApi_CreatesAndReadsRealJob_WithoutInventingPublicationDate()
    {
        var connectionString = Environment.GetEnvironmentVariable("VAGAFLOW_TEST_DB_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return; // CI always supplies a disposable PostgreSQL database.
        }

        var options = new DbContextOptionsBuilder<VagaFlowDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using (var context = new VagaFlowDbContext(options))
        {
            await context.Database.MigrateAsync();
        }

        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, config) =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:VagaFlow"] = connectionString
                })));
        using var client = factory.CreateClient();

        var canonicalUrl = $"https://careers.example.org/jobs/{Guid.NewGuid():N}";
        var valid = new
        {
            company = "Empresa Sintética VF",
            title = "Desenvolvedor de Testes",
            url = canonicalUrl + "?utm_source=ci",
            sourcePortal = "portal-ficticio",
            description = "Descrição de teste, não é vaga real.",
            area = "it",
            workArrangement = "remote",
            city = "Teresina",
            state = "PI"
        };

        using var created = await client.PostAsJsonAsync("/api/v1/jobs/manual", valid);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);

        using var createdJson = await JsonDocument.ParseAsync(await created.Content.ReadAsStreamAsync());
        var jobId = createdJson.RootElement.GetProperty("id").GetGuid();
        Assert.Equal(JsonValueKind.Null, createdJson.RootElement.GetProperty("publishedAtUtc").ValueKind);
        Assert.Equal("pendingReview", createdJson.RootElement.GetProperty("verificationState").GetString());

        try
        {
            using var fetched = await client.GetAsync($"/api/v1/jobs/{jobId}");
            Assert.Equal(HttpStatusCode.OK, fetched.StatusCode);

            using var duplicate = await client.PostAsJsonAsync("/api/v1/jobs/manual", new
            {
                valid.company,
                valid.title,
                url = canonicalUrl,
                valid.sourcePortal,
                valid.description,
                valid.area,
                valid.workArrangement,
                valid.city,
                valid.state
            });
            Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);

            using var unsafeUrl = await client.PostAsJsonAsync("/api/v1/jobs/manual", new
            {
                valid.company,
                valid.title,
                url = "https://localhost/admin",
                valid.sourcePortal
            });
            Assert.Equal(HttpStatusCode.BadRequest, unsafeUrl.StatusCode);

            using var invalidDate = await client.PostAsJsonAsync("/api/v1/jobs/manual", new
            {
                valid.company,
                valid.title,
                url = $"https://careers.example.org/jobs/{Guid.NewGuid():N}",
                valid.sourcePortal,
                publishedAtUtc = "2026-11-01T00:00:00Z",
                publicationDateVerified = false
            });
            Assert.Equal(HttpStatusCode.BadRequest, invalidDate.StatusCode);

            using var recentOnly = await client.GetAsync("/api/v1/jobs?publishedSince=2026-11-01");
            Assert.Equal(HttpStatusCode.OK, recentOnly.StatusCode);
            using var recentJson = await JsonDocument.ParseAsync(await recentOnly.Content.ReadAsStreamAsync());
            Assert.DoesNotContain(recentJson.RootElement.EnumerateArray(),
                item => item.GetProperty("id").GetGuid() == jobId);
        }
        finally
        {
            await using var context = new VagaFlowDbContext(options);
            var persisted = await context.Jobs.FindAsync(jobId);
            if (persisted is not null)
            {
                context.Jobs.Remove(persisted);
                await context.SaveChangesAsync();
            }
        }
    }

}
