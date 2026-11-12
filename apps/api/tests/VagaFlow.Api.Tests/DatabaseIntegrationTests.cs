using Microsoft.EntityFrameworkCore;
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
}
