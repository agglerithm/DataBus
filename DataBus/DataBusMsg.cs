using System;
using DataBus.Data.Entities;
using Newtonsoft.Json;

namespace DataBus
{
    public class DataBusMsg<T> where T : class
    {
        public DataBusMsg(T obj)
        {
            Payload = obj;
            DateCreated = DateTime.Now;
        }

        public DataBusMsg(MessageQueueItem item)
        {
            Payload = JsonConvert.DeserializeObject<T>(item.Publication.Payload);
            DateCreated = item.Publication.DateCreated;
            MaxAttempts = item.Subscription.MaxAttempts;
            RetryDelay = item.Subscription.RetryDelay;
            LastAttempt = item.LastAttempt;
        }

        public DateTime? LastAttempt { get; set; }

        public long RetryDelay { get; set; }

        public int MaxAttempts { get; set; }

        public string MessageType
        {
            get { return typeof (T).Name; }
        }
        public DateTime DateCreated { get; private set; }
        public T Payload { get; private set; }
    }
}