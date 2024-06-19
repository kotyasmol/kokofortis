using System;
using System.Collections.Generic;

namespace Tests;

public partial class OidsForDevice
{
    public int? Key { get; set; }

    public int? DeviceTypeId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public int? OkValue { get; set; }

    public string? OkValueText { get; set; }

    public int? BadValue { get; set; }

    public string? BadValueText { get; set; }

    public int? Timeout { get; set; }

    public int? Invertible { get; set; }

    public int? Enable { get; set; }
}
