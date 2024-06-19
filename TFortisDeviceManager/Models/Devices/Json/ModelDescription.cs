using System.Collections.Generic;

namespace TFortisDeviceManager.Models
{
    public class JsonModelDescription
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<JsonPortDescription> Ports { get; }
        public IReadOnlyList<JsonContactDescription> Contacts { get; }
        public bool HasUps { get; }

        public JsonModelDescription(int id, string name, bool hasUps, IReadOnlyList<JsonPortDescription> ports, IReadOnlyList<JsonContactDescription> contacts)
        {
            Id = id;
            Name = name;
            Ports = ports;
            HasUps = hasUps;
            Contacts = contacts;
        }
    }


    public class JsonPortDescription 
    {
        public bool IsSfp { get; }
        public PoeType Poe { get; }
        public uint NumberOf { get; }

        public JsonPortDescription(bool isSfp, PoeType poe, uint numberOf)
        {
            IsSfp = isSfp;
            Poe = poe;
            NumberOf = numberOf;
        }
    }


    public class JsonContactDescription
    {
        public ContactType Type { get; }
        public uint NumberOf { get; }

        public JsonContactDescription(ContactType type, uint numberOf)
        {
            Type = type;
            NumberOf = numberOf;
        }
    }
}
