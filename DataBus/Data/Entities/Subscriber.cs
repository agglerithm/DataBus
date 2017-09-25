using System;

namespace DataBus.Data.Entities
{
    public class Subscriber
    {
        public Guid idSubscriber { get; set; }
        public Guid idPublisher { get; set; }
        public Guid idChannel { get; set; }
        public Guid idMessageType { get; set; }
        public MessageType Type { get; set; }
        public TimeSpan Delay { get; set; }
    }
}