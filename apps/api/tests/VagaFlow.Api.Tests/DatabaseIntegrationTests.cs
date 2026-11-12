using Microsoft.EntityFrameworkCore;
using VagaFlow.Api.Persistence;
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
        await using (var db = new VagaFlowDbContext(options))
        {
            await db.Database.MigrateAsync();
            var applied = await db.Database.GetAppliedMigrationsAsync();

            Assert.Contains("20261112190000_InitialInfrastructure", applied);

            db.AppSettings.Add(new AppSetting
            {
                Key = uniqueKey,
                Value = "test-ok",
                UpdatedAtUtc = DateTime.UtcNow
            });

            Assert.Equal(1, await db.SaveChangesAsync());
        }

        await using (var db = new VagaFlowDbContext(options))
        {
            var saved = await db.AppSettings.AsNoTracking()
                .SingleAsync(setting => setting.Key == uniqueKey);

            Assert.Equal("test-ok", saved.Value);
            db.AppSettings.Remove(saved);
            Assert.Equal(1, await db.SaveChangesAsync());
        }
    }
}
