using System;

namespace DataBus.Data.Entities
{
    public class Publication
    {
        public Guid idPublication { get; set; }
        public string MessageType { get; set; }
        public string Payload { get; set; }
        public DateTime DateCreated { get; set; }
    }
}