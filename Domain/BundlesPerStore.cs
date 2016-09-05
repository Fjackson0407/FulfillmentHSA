using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public   class BundlesPerStore
    {
        public string  PONumber { get; set; }
        public string  Store { get; set; }
        public int QuantityOrdered { get; set; }
        public int  Bundles { get; set; }
    }
}
