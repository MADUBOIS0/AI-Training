using System;
using System.Collections.Generic;

namespace ApiBoard.Core.Entities;

public class HealthCheck
{
    public Guid Id { get; set; }
    public Guid ApiRequestId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IntervalSeconds { get; set; }
    public bool IsEnabled { get; set; }
    public string AssertionsJson { get; set; } = "{}";
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ApiRequest ApiRequest { get; set; } = null!;
    public ICollection<HealthResult> Results { get; set; } = new List<HealthResult>();
}

