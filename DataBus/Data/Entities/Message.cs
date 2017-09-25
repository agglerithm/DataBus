using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBus.Data.Entities
{
    public class Message
    {
        public Guid idMessage { get; set; }
        public Guid idMessageType { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }
}
