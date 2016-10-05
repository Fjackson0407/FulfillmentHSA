using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public   class SummaryForInverntory
    {
        public String Store { get; set; }
        public string  PO { get; set; }
        public string  UPC { get; set; }
        public string  DPCIFull { get; set; }
        public int QuantityOrdered { get; set; }
        public string DPCI {
            get

            {
                if (string.IsNullOrEmpty( DPCIFull ))
                {
                    return string.Empty;
                }
                return DPCIFull.Substring(DPCIFull.Length - 4, 4 );
            }
                }

        public int Bundles {
            get
            {
                return QuantityOrdered / 25;
            }
                
                }

        public double  ItemWeight { get; set; }
    }
}
