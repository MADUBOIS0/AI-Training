using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class ApiRequestConfiguration : IEntityTypeConfiguration<ApiRequest>
{
    public void Configure(EntityTypeBuilder<ApiRequest> builder)
    {
        builder.ToTable("ApiRequests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Method)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.Url)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(r => r.HeadersJson)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasDefaultValue("{}");

        builder.Property(r => r.Body)
            .HasColumnType("TEXT");

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.HasMany(r => r.ResponseLogs)
            .WithOne(l => l.ApiRequest)
            .HasForeignKey(l => l.ApiRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.HealthChecks)
            .WithOne(h => h.ApiRequest)
            .HasForeignKey(h => h.ApiRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

