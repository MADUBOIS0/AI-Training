using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class HealthResultConfiguration : IEntityTypeConfiguration<HealthResult>
{
    public void Configure(EntityTypeBuilder<HealthResult> builder)
    {
        builder.ToTable("HealthResults");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(r => r.StatusCode)
            .IsRequired();

        builder.Property(r => r.DurationMs)
            .IsRequired();

        builder.Property(r => r.FailureReason)
            .HasColumnType("TEXT");

        builder.Property(r => r.CreatedAt)
            .IsRequired();
    }
}

