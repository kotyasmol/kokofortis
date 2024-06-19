namespace TFortisDeviceManager.Models
{
    public class Contact
    {
        public ContactType Type { get; set; }
    }

    public enum ContactType
    {
        Input,
        Output
    }
}
