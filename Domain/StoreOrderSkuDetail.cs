using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
 public    class StoreOrderSkuDetail
    {
        public String UPC { get; set; }
        public string  DPCI { get; set; }

        public string  ProductDescription { get; set; }

        public int CustomerLineNumber { get; set; }

    }
}
