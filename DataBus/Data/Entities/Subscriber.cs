using System;

namespace DataBus.Data.Entities
{
    public class Subscriber
    {
        public Guid idSubscriber { get; set; }
        public string Assembly { get; set; }
        public Guid idChannel { get; set; }
    }
}