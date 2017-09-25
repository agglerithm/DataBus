using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBus.Data.Entities
{
    public class Publisher
    {
        public Guid idPublisher
        {
            get; set;  
        }
        public Guid SourceEndpoint { get; set; }
        public string MessageType { get; set; }

    }
}
