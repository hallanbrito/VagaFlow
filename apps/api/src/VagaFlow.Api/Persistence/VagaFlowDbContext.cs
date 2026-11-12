using Microsoft.EntityFrameworkCore;

namespace VagaFlow.Api.Persistence;

public sealed class VagaFlowDbContext(DbContextOptions<VagaFlowDbContext> options) : DbContext(options)
{
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.ToTable("app_settings");
            entity.HasKey(setting => setting.Key).HasName("pk_app_settings");

            entity.Property(setting => setting.Key)
                .HasColumnName("key")
                .HasMaxLength(128)
                .IsRequired();

            entity.Property(setting => setting.Value)
                .HasColumnName("value")
                .HasMaxLength(2048)
                .IsRequired();

            entity.Property(setting => setting.UpdatedAtUtc)
                .HasColumnName("updated_at_utc")
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        });
    }
}
