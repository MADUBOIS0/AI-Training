using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class ResponseLogConfiguration : IEntityTypeConfiguration<ResponseLog>
{
    public void Configure(EntityTypeBuilder<ResponseLog> builder)
    {
        builder.ToTable("ResponseLogs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.StatusCode)
            .IsRequired();

        builder.Property(l => l.DurationMs)
            .IsRequired();

        builder.Property(l => l.HeadersJson)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasDefaultValue("{}");

        builder.Property(l => l.Body)
            .HasColumnType("TEXT");

        builder.Property(l => l.CreatedAt)
            .IsRequired();
    }
}

