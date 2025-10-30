using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiBoard.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBoard.Infrastructure;

public class AppDbContext : DbContext
{
    private static readonly SqlitePragmaInterceptor PragmaInterceptor = new();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Environment> Environments => Set<Environment>();
    public DbSet<EnvironmentVar> EnvironmentVars => Set<EnvironmentVar>();
    public DbSet<RequestCollection> RequestCollections => Set<RequestCollection>();
    public DbSet<ApiRequest> ApiRequests => Set<ApiRequest>();
    public DbSet<HealthCheck> HealthChecks => Set<HealthCheck>();
    public DbSet<ResponseLog> ResponseLogs => Set<ResponseLog>();
    public DbSet<HealthResult> HealthResults => Set<HealthResult>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(PragmaInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                continue;
            }

            switch (entry.Entity)
            {
                case Environment environment:
                    if (entry.State == EntityState.Added)
                    {
                        environment.CreatedAt = environment.CreatedAt == default ? utcNow : environment.CreatedAt;
                    }

                    environment.UpdatedAt = utcNow;
                    break;

                case RequestCollection collection:
                    if (entry.State == EntityState.Added)
                    {
                        collection.CreatedAt = collection.CreatedAt == default ? utcNow : collection.CreatedAt;
                    }

                    collection.UpdatedAt = utcNow;
                    break;

                case ApiRequest request:
                    if (entry.State == EntityState.Added)
                    {
                        request.CreatedAt = request.CreatedAt == default ? utcNow : request.CreatedAt;
                    }

                    request.UpdatedAt = utcNow;
                    break;

                case HealthCheck healthCheck:
                    if (entry.State == EntityState.Added)
                    {
                        healthCheck.CreatedAt = healthCheck.CreatedAt == default ? utcNow : healthCheck.CreatedAt;
                    }

                    healthCheck.UpdatedAt = utcNow;
                    break;

                case ResponseLog responseLog when entry.State == EntityState.Added:
                    responseLog.CreatedAt = responseLog.CreatedAt == default ? utcNow : responseLog.CreatedAt;
                    break;

                case HealthResult healthResult when entry.State == EntityState.Added:
                    healthResult.CreatedAt = healthResult.CreatedAt == default ? utcNow : healthResult.CreatedAt;
                    break;
            }
        }
    }
}

