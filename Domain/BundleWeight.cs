using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public  class BundleWeight
    {
        public Guid  Id { get; set; }
        public string  CardType { get; set; }
        public Double Weigtht { get; set; }
        public DateTime DTS { get; set; }
        public bool InUse { get; set; }
        public virtual Store store { get; set;  }
    }
}
