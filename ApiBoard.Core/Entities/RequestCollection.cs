using System;
using System.Collections.Generic;

namespace ApiBoard.Core.Entities;

public class RequestCollection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<ApiRequest> Requests { get; set; } = new List<ApiRequest>();
}

