using System;

namespace DataBus.Data.Entities
{
    public class Subscription
    {
        public Guid idSubscription;
        public Subscriber Subscriber { get; set; }
        public DataBusChannel Channel { get; set; }
        public long RetryDelay { get; set; }
        public int MaxAttempts { get; set; } 

    }
}