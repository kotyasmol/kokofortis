namespace TFortisDeviceManager.Models
{
    public class Sensor
    {
        public int? Key { get; set; }
        public string? Ip { get; set; }
        public string? DeviceName { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? OkValue { get; set; }
        public string? OkValueText { get; set; }
        public int? BadValue { get; set; }
        public string? BadValueText { get; set; }
        public bool Invertible { get; set; }
        public bool Invert { get; set; }
        public int Timeout { get; set; }
        public bool Enable { get; set; }

        public override string ToString()
        {
            return DeviceName + " " + Ip + " " + Name + " " + Description;
        }
    }
}
