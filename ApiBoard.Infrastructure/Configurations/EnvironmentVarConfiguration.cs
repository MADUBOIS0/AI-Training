using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class EnvironmentVarConfiguration : IEntityTypeConfiguration<EnvironmentVar>
{
    public void Configure(EntityTypeBuilder<EnvironmentVar> builder)
    {
        builder.ToTable("EnvironmentVars");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Key)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Value)
            .IsRequired();

        builder.Property(v => v.IsSecret)
            .IsRequired();

        builder.HasIndex(v => new { v.EnvironmentId, v.Key })
            .IsUnique();
    }
}

