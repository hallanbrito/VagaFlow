using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using VagaFlow.Api.Jobs;
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

        modelBuilder.Entity<JobListing>(entity =>
        {
            entity.ToTable("job_listings");
            entity.HasKey(job => job.Id).HasName("pk_job_listings");
            entity.Property(job => job.Id).HasColumnName("id");
            entity.Property(job => job.Company).HasColumnName("company").HasMaxLength(160).IsRequired();
            entity.Property(job => job.Title).HasColumnName("title").HasMaxLength(180).IsRequired();
            entity.Property(job => job.OriginalUrl).HasColumnName("original_url").HasMaxLength(2048).IsRequired();
            entity.Property(job => job.CanonicalUrl).HasColumnName("canonical_url").HasMaxLength(2048).IsRequired();
            entity.Property(job => job.SourcePortal).HasColumnName("source_portal").HasMaxLength(120).IsRequired();
            entity.Property(job => job.Description).HasColumnName("description").HasColumnType("text").IsRequired();
            entity.Property(job => job.Area).HasColumnName("area").HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.Property(job => job.WorkArrangement).HasColumnName("work_arrangement").HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.Property(job => job.City).HasColumnName("city").HasMaxLength(120);
            entity.Property(job => job.State).HasColumnName("state").HasMaxLength(2);
            entity.Property(job => job.PublishedAtUtc).HasColumnName("published_at_utc").HasColumnType("timestamp with time zone");
            entity.Property(job => job.FirstSeenAtUtc).HasColumnName("first_seen_at_utc").HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(job => job.VerificationState).HasColumnName("verification_state").HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(job => job.CanonicalUrl).IsUnique().HasDatabaseName("ux_job_listings_canonical_url");
        });
    }
}
