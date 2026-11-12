using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VagaFlow.Api.Persistence.Migrations;

[DbContext(typeof(VagaFlowDbContext))]
public sealed class VagaFlowDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.15")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

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
