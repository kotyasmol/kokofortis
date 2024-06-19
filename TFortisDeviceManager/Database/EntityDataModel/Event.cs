using TFortisDeviceManager.Models;

namespace TFortisDeviceManager;

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

    public EventModel ToEventModel()
    {
        EventModel model = new()
        {
            Id=Id,
            Time = Time,
            Ip = Ip,
            DeviceName = DeviceName,
            SensorName = SensorName,
            SensorValueText = SensorValueText,
            Age = Age,
            Description = Description,
            Status = Status,
            DeviceLocation = DeviceLocation,
            DeviceDescription = DeviceDescription
        };
        return model;

    }

}
