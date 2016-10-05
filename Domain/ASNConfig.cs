using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public   class ASNConfig
    {
        public Guid ID { get; set; }
        public string  Name { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
        public DateTime DTS { get; set; }

    }
}
