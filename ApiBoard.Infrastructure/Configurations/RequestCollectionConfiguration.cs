using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBoard.Infrastructure.Configurations;

public class RequestCollectionConfiguration : IEntityTypeConfiguration<RequestCollection>
{
    public void Configure(EntityTypeBuilder<RequestCollection> builder)
    {
        builder.ToTable("RequestCollections");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Notes)
            .HasMaxLength(2000);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasMany(c => c.Requests)
            .WithOne(r => r.Collection)
            .HasForeignKey(r => r.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

