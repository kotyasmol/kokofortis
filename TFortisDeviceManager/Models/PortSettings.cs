namespace TFortisDeviceManager.Models
{
    public class PortSettings
    {
        public int Id { get; }
        public PoeType Poe { get; }
        public bool EnablePort { get; set; }
        public bool EnablePoe { get; set; }


        public PortSettings(int id, PoeType poe, bool enablePort, bool enablePoe)
        {
            Id = id;
            Poe = poe;
            EnablePort = enablePort;
            EnablePoe = enablePoe;
        }
    }
}
