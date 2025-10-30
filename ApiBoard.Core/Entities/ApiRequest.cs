using System;
using System.Collections.Generic;

namespace ApiBoard.Core.Entities;

public class ApiRequest
{
    public Guid Id { get; set; }
    public Guid CollectionId { get; set; }
    public Guid? EnvironmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string Url { get; set; } = string.Empty;
    public string HeadersJson { get; set; } = "{}";
    public string? Body { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public RequestCollection Collection { get; set; } = null!;
    public Environment? Environment { get; set; }
    public ICollection<ResponseLog> ResponseLogs { get; set; } = new List<ResponseLog>();
    public ICollection<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();
}

