using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VagaFlow.Api.Persistence;

/// <summary>
/// Keeps migrations explicit. No database is created or migrated at API startup.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VagaFlowDbContext>
{
    public VagaFlowDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__VagaFlow");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Informe ConnectionStrings__VagaFlow para usar o dotnet ef. Consulte infra/README.md.");
        }

        var options = new DbContextOptionsBuilder<VagaFlowDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new VagaFlowDbContext(options);
    }
}
