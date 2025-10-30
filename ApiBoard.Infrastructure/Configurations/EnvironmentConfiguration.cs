using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class EnvironmentConfiguration : IEntityTypeConfiguration<Environment>
{
    public void Configure(EntityTypeBuilder<Environment> builder)
    {
        builder.ToTable("Environments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasMany(e => e.Variables)
            .WithOne(v => v.Environment)
            .HasForeignKey(v => v.EnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Requests)
            .WithOne(r => r.Environment)
            .HasForeignKey(r => r.EnvironmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

