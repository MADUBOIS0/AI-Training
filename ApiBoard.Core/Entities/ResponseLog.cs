using System;

namespace ApiBoard.Core.Entities;

public class ResponseLog
{
    public Guid Id { get; set; }
    public Guid ApiRequestId { get; set; }
    public int StatusCode { get; set; }
    public int DurationMs { get; set; }
    public string HeadersJson { get; set; } = "{}";
    public string? Body { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ApiRequest ApiRequest { get; set; } = null!;
}

