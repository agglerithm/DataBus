using System;

namespace DataBus.Data.Entities
{
    public class DataBusChannel
    {
        public Guid idChannel { get; set; }
        public string PublisherAssembly { get; set; }
        public string MessageType { get; set; }
    }
}