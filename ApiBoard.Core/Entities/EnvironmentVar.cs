using System;

namespace ApiBoard.Core.Entities;

public class EnvironmentVar
{
    public Guid Id { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsSecret { get; set; }

    public Environment Environment { get; set; } = null!;
}

