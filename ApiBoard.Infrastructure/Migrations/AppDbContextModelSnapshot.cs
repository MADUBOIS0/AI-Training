using System;
using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using EnvironmentEntity = ApiBoard.Core.Entities.Environment;

namespace ApiBoard.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
public partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

        modelBuilder.Entity<EnvironmentEntity>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.Property<string>("Notes")
                .HasMaxLength(2000)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("UpdatedAt")
                .HasColumnType("TEXT");

            b.ToTable("Environments");
        });

        modelBuilder.Entity<EnvironmentVar>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<Guid>("EnvironmentId")
                .HasColumnType("TEXT");

            b.Property<bool>("IsSecret")
                .HasColumnType("INTEGER");

            b.Property<string>("Key")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.Property<string>("Value")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("EnvironmentId", "Key")
                .IsUnique();

            b.ToTable("EnvironmentVars");

            b.HasOne<EnvironmentEntity>()
                .WithMany(e => e.Variables)
                .HasForeignKey("EnvironmentId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<RequestCollection>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.Property<string>("Notes")
                .HasMaxLength(2000)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("UpdatedAt")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("RequestCollections");
        });

        modelBuilder.Entity<ApiRequest>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<Guid>("CollectionId")
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<Guid?>("EnvironmentId")
                .HasColumnType("TEXT");

            b.Property<string>("HeadersJson")
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValue("{}");

            b.Property<string>("Method")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.Property<string>("Url")
                .IsRequired()
                .HasMaxLength(2048)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("UpdatedAt")
                .HasColumnType("TEXT");

            b.Property<string>("Body")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("CollectionId");

            b.HasIndex("EnvironmentId");

            b.ToTable("ApiRequests");

            b.HasOne<RequestCollection>()
                .WithMany(c => c.Requests)
                .HasForeignKey("CollectionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne<EnvironmentEntity>()
                .WithMany(e => e.Requests)
                .HasForeignKey("EnvironmentId")
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<HealthCheck>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<Guid>("ApiRequestId")
                .HasColumnType("TEXT");

            b.Property<string>("AssertionsJson")
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValue("{}");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<int>("IntervalSeconds")
                .HasColumnType("INTEGER");

            b.Property<bool>("IsEnabled")
                .HasColumnType("INTEGER");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("UpdatedAt")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("ApiRequestId");

            b.ToTable("HealthChecks");

            b.HasOne<ApiRequest>()
                .WithMany(r => r.HealthChecks)
                .HasForeignKey("ApiRequestId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<HealthResult>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<int>("DurationMs")
                .HasColumnType("INTEGER");

            b.Property<Guid>("HealthCheckId")
                .HasColumnType("TEXT");

            b.Property<string>("FailureReason")
                .HasColumnType("TEXT");

            b.Property<int>("StatusCode")
                .HasColumnType("INTEGER");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("HealthCheckId");

            b.ToTable("HealthResults");

            b.HasOne<HealthCheck>()
                .WithMany(h => h.Results)
                .HasForeignKey("HealthCheckId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<ResponseLog>(b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<Guid>("ApiRequestId")
                .HasColumnType("TEXT");

            b.Property<string>("Body")
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<int>("DurationMs")
                .HasColumnType("INTEGER");

            b.Property<string>("HeadersJson")
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValue("{}");

            b.Property<int>("StatusCode")
                .HasColumnType("INTEGER");

            b.HasKey("Id");

            b.HasIndex("ApiRequestId");

            b.ToTable("ResponseLogs");

            b.HasOne<ApiRequest>()
                .WithMany(r => r.ResponseLogs)
                .HasForeignKey("ApiRequestId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<EnvironmentEntity>()
            .Navigation(e => e.Requests);

        modelBuilder.Entity<EnvironmentEntity>()
            .Navigation(e => e.Variables);

        modelBuilder.Entity<ApiRequest>()
            .Navigation(r => r.HealthChecks);

        modelBuilder.Entity<ApiRequest>()
            .Navigation(r => r.ResponseLogs);

        modelBuilder.Entity<HealthCheck>()
            .Navigation(h => h.Results);
    }
}
