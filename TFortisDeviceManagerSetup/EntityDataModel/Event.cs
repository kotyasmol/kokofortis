namespace TFortisDeviceManagerSetup;

public partial class Event
{
    public int Id { get; set; }

    public string Time { get; set; } = null!;

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
