namespace TFortisDeviceManager;

public partial class DeviceForMonitoring
{
    public int Key { get; set; }

    public int DeviceTypeId { get; set; }

    public string Ip { get; set; } = null!;

    public string Mac { get; set; } = null!;

    public int Uptime { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string Community { get; set; } = null!;

    public int? SendEmail { get; set; }

    public string? SerialNumber { get; set; }
}
