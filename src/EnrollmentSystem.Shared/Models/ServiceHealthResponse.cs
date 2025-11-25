using System;

namespace EnrollmentSystem.Shared.Models;

public class ServiceHealthResponse
{
    public string Status { get; set; } = "Healthy";
    public string Service { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}