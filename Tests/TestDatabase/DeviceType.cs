using System;
using System.Collections.Generic;

namespace Tests;

public partial class DeviceType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? PortsPoe { get; set; }

    public int? PortsWithoutPoe { get; set; }

    public int? PortsSfp { get; set; }

    public int? PortsUplink { get; set; }

    public int? Inputs { get; set; }

    public int? Outputs { get; set; }

    public int? Ups { get; set; }
}
