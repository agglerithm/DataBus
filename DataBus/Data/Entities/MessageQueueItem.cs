using System;

namespace DataBus.Data.Entities
{
    public class MessageQueueItem
    {
        public Guid idMessageQueueItem { get; set; }
        public Guid idChannel { get; set; }
        public DataBusChannel Channel { get; set; }
        public Guid idMessageType { get; set; }
        public MessageType Type { get; set; }
        public Guid receiverEndpoint { get; set; } 
        public Endpoint Receiver { get; set; }
        public int AttemptCount { get; set; }
        public DateTime LastAttempt { get; set; }
        public MessageStatus Status { get; set; }
}
}