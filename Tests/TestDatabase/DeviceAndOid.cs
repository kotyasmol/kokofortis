using System;
using System.Collections.Generic;

namespace Tests;

public partial class DeviceAndOid
{
    public int Key { get; set; }

    public int? DeviceForMonitoringKey { get; set; }

    public int? OidForDeviceKey { get; set; }

    public int? Timeout { get; set; }

    public int? Invertible { get; set; }

    public int? Invert { get; set; }

    public int? Enable { get; set; }

    public int? SendEmail { get; set; }
}
