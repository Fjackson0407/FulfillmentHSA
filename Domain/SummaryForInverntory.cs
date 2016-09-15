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
        public int QTYCards { get; set; }

        public string  DPCI { get; set; }
        public int Pkgs { get; set; }
    }
}
