using System;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager;

public partial class OidsForDevice
{
    public int Key { get; set; }

    public int DeviceTypeId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public int? OkValue { get; set; }

    public string? OkValueText { get; set; }

    public int? BadValue { get; set; }

    public string? BadValueText { get; set; }

    public int Timeout { get; set; }

    public int Invertible { get; set; }

    public int Enable { get; set; }


    public OidModel ToOidModel()
    {
        OidModel model = new()
        {
            Key = Key,
            Name = Name,
            Description = Description,
            Timeout = Timeout,
            Invertible = Convert.ToBoolean(Invertible),
            Enable = Convert.ToBoolean(Enable)
        };
        return model;
    }

    public OidModel ToOidModel(bool invert)
    {
        OidModel model = new()
        {
            Key = Key,
            Name = Name,
            Description = Description,
            Timeout = Timeout,
            Invertible = Convert.ToBoolean(Invertible),
            Invert = invert,
            Enable = Convert.ToBoolean(Enable)
        };
        return model;
    }

}
