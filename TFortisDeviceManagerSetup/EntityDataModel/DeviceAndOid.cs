using System.ComponentModel.DataAnnotations.Schema;

namespace TFortisDeviceManagerSetup;

public partial class DeviceAndOid
{
    public int Key { get; set; }

    public int DeviceForMonitoringKey { get; set; }

    public int OidForDeviceKey { get; set; }

    public int Timeout { get; set; }

    public bool Invertible { get; set; }

    public bool Invert { get; set; }

    public bool Enable { get; set; }

    public bool SendEmail { get; set; }
}
