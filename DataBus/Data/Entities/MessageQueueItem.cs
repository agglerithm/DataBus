using System;

namespace DataBus.Data.Entities
{
    public class MessageQueueItem
    {
        public Guid id;
        public string MessageType { get; set; }
        public Publication Publication { get; set; } 
        public Subscription Subscription { get; set; }
        public int AttemptCount { get; set; }
        public DateTime LastAttempt { get; set; }
    }
}