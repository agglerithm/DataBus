using System;

namespace DataBus.Data.Entities
{
    public class DataBusChannel
    {
        public Guid idChannel { get; set; }
        public Guid sourceEndpoint { get; set; }
        public Guid destinationEndpoint { get; set; }
        public Guid idMessageType { get; set; }
        public MessageType Type { get; set; }
    }
}