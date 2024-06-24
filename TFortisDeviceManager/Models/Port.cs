namespace TFortisDeviceManager.Models
{
    public class Port
    {
        public bool IsSfp { get; }
        public PoeType Poe { get; }

        public Port(bool isSfp, PoeType poe)
        {
            IsSfp = isSfp;
            Poe = poe;
        }
    }

    public enum PoeType
    {
        No,
        Poe,
        HiPoe
    }
} 