using System;
using System.Collections.Generic;

namespace Tests;

/// <summary>
/// MonitoringEvents
/// </summary>
public partial class Event
{
    public int Id { get; set; }

    public TimeOnly Time { get; set; }

    public string DeviceName { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string SensorName { get; set; } = null!;

    public string SensorValueText { get; set; } = null!;

    public string? Age { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string? DeviceLocation { get; set; }

    public string? DeviceDescription { get; set; }
}
