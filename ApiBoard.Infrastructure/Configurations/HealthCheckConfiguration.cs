using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class HealthCheckConfiguration : IEntityTypeConfiguration<HealthCheck>
{
    public void Configure(EntityTypeBuilder<HealthCheck> builder)
    {
        builder.ToTable("HealthChecks");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.IntervalSeconds)
            .IsRequired();

        builder.Property(h => h.IsEnabled)
            .IsRequired();

        builder.Property(h => h.AssertionsJson)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasDefaultValue("{}");

        builder.Property(h => h.CreatedAt)
            .IsRequired();

        builder.Property(h => h.UpdatedAt)
            .IsRequired();

        builder.HasMany(h => h.Results)
            .WithOne(r => r.HealthCheck)
            .HasForeignKey(r => r.HealthCheckId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

