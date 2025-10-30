using System;

using ApiBoard.Core.Enums;

namespace ApiBoard.Core.Entities;

public class HealthResult
{
    public Guid Id { get; set; }
    public Guid HealthCheckId { get; set; }
    public HealthStatus Status { get; set; }
    public int StatusCode { get; set; }
    public int DurationMs { get; set; }
    public string? FailureReason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public HealthCheck HealthCheck { get; set; } = null!;
}

